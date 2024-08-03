using Application.Common.Interfaces;
using Application.Common.Model;
using Application.User.Commands.CreateRole;
using Application.User.Commands.CreateUser;
using Application.User.Commands.LoginUser;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public IdentityService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this._roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        
        public async Task<Result> CreateRoleAsync(RoleCommand roleCommand, CancellationToken cancellationToken)
        {
            try
            {
                IdentityResult finalResult = new IdentityResult();
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ApplicationRole appRole = new ApplicationRole
                    {
                        Name = roleCommand.RoleName,
                        Description = roleCommand.Description
                    };

                    IdentityResult result = await _roleManager.CreateAsync(appRole);
                    if (result.Succeeded)
                    {
                        ApplicationRole role = await _roleManager.FindByNameAsync(roleCommand.RoleName);
                        //IdentityResult result = await _roleManager.FindByNameAsync(roleCommand.RoleName);
                        foreach (var permission in roleCommand.Permissions)
                        {
                            if (Enum.IsDefined(typeof(Domain.Enums.Permission), permission.PermissionValue))
                            {
                                Claim claim = new Claim(CustomClaimType.Permission, permission.PermissionValue.ToString());
                                finalResult = await _roleManager.AddClaimAsync(role, claim);
                            }
                        }
                    }
                    scope.Complete();
                }
                return finalResult.ToApplicationResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> CreateUserAsync(UserCommand userCommand, CancellationToken cancellationToken)
        {
            IdentityResult finalResult = new IdentityResult();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = userCommand.UserName,
                    Address = userCommand.Address,
                    Email = userCommand.Email,
                    IsActive = true

                };
                IdentityResult user = await _userManager.CreateAsync(appUser);
                if(user.Succeeded)
                {
                    IdentityResult addPasswordHash = await _userManager.AddPasswordAsync(appUser, userCommand.Password);
                    if(addPasswordHash.Succeeded)
                    {
                        finalResult = await _userManager.AddToRolesAsync(appUser, userCommand.Roles);
                    }
                }
                scope.Complete();
            }
            return finalResult.ToApplicationResult();
        }

        public async Task<TokenResult> LoginUserAsync(LoginUserCommand loginUserCommand, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByNameAsync(loginUserCommand.Username);
            if(identityUser == null)
            {
                return new TokenResult { Error = "User does not exist", StatusCode = 401, Succeeded = false };
            }
            else if(identityUser.IsActive == false)
            {
                return new TokenResult { Error = "User is deactivated", StatusCode = 401, Succeeded = false };
            }
            else
            {
                var result = await _signInManager.CheckPasswordSignInAsync(identityUser, loginUserCommand.Password,true);

                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        identityUser.IsActive = false;
                        return new TokenResult { Error = "User is locked, please contact admin", StatusCode = 401, Succeeded = false };
                    }
                    identityUser.AccessFailedCount++;
                    return new TokenResult { Error = "Credential is Invalid", StatusCode = 401, Succeeded = false };
                }
                else
                {
                    List<Claim> claims = await ConstructUserClaimAsync(identityUser);
                    JwtSecurityToken token = GenerateJwtTokenAsync(claims);
                    TokenResult tokenResult = new TokenResult
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,Succeeded = true

                    };
                    return tokenResult;


                }
            }
        }

        private async Task<List<Claim>> ConstructUserClaimAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> assignedRoles = roles.Select(role => new Claim("role", role)).ToList();
            List<Claim> userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
            List<Claim> claims = userClaims.Union(assignedRoles).ToList();

            foreach (var role in roles)
            {
                ApplicationRole appRole = await _roleManager.FindByNameAsync(role);
                IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(appRole);
                claims = claims.Union(roleClaims).ToList();
            }

            claims = new List<Claim>(claims)
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
            };
            return claims;
        }
        private  JwtSecurityToken GenerateJwtTokenAsync(List<Claim> claims)
        {
            var jwtKey =  _configuration.GetValue<string>("Tokens:JwtKey");
            var jwtIssuer = _configuration.GetValue<string>("Tokens:JwtIssuer");
            var jwtAudience = _configuration.GetValue<string>("Tokens:JwtAudience");
            var jwtValidMinutes = _configuration.GetValue<string>("Tokens:JwtValidMinutes");
            var token = new JwtSecurityToken();
            if(jwtKey != null && jwtIssuer != null && jwtAudience != null && jwtValidMinutes != null)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                token = new JwtSecurityToken
                (
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtValidMinutes)),
                    signingCredentials: signingCred
                );
            return token;
            }
            else
            {
                throw new InvalidOperationException("JWT configuration is missing or invalid.");
            }
        }

    }
}
