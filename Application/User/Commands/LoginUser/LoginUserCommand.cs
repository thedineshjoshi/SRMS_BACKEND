using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.LoginUser
{
    public class LoginUserCommand:IRequest<TokenResult>
    {
        public string Username { get; set; }
        public string  Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResult>
       
    {
        private readonly IIdentityService _identityService;
        public LoginUserCommandHandler (IIdentityService identityService)
        {
            this._identityService = identityService;
        }
        public async Task<TokenResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginUserAsync(request, cancellationToken);
        }
    }
}
