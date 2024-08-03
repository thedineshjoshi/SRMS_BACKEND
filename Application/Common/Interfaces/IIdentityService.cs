using Application.Common.Model;
using Application.User.Commands.CreateRole;
using Application.User.Commands.CreateUser;
using Application.User.Commands.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> CreateRoleAsync(RoleCommand roleCommand, CancellationToken cancellationToken);
        Task<Result> CreateUserAsync(UserCommand userCommand, CancellationToken cancellationToken);
        Task<TokenResult> LoginUserAsync(LoginUserCommand loginUserCommand, CancellationToken cancellationToken);




    }
}
