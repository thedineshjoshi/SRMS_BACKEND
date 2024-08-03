using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.CreateRole
{
    public class RoleCommand:IRequest<Result>
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }
    public class RoleCommandHandle :IRequestHandler<RoleCommand,Result>
    {
        private readonly IIdentityService _identityService;
        public RoleCommandHandle(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Result>Handle(RoleCommand request,CancellationToken cancellationToken)
        {
            return await _identityService.CreateRoleAsync(request, cancellationToken);
        }

    }
}
