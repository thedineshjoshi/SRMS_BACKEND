using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.CreateUser
{
    public class UserCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
        
    }
    public class UserCommandHandler : IRequestHandler<UserCommand, Result>
    { 
        private readonly IIdentityService _identityService;
    
        public UserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Result> Handle(UserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.CreateUserAsync(request, cancellationToken);
        }
    }
}
