using Application.Common.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StudentRegistrations.Commands.CreateStudent
{
    public class CreateStudentCommand:IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string GradeLevel { get; set; }
        public string Address { get; set; }
        public string GuardianFullName { get; set; }
        public int GuardianContactNumber { get; set; }
        public string StudentEmailAddress { get; set; }
    }
    public class CreateStudentRegistrationHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IApplicationDbContext _dbcontext;

        public CreateStudentRegistrationHandler(IApplicationDbContext context)
        {
            this._dbcontext = context;
        }
        public async Task<Guid> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            var entity = new StudentRegistration
            {    FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth,
                Gender = command.Gender,
                GradeLevel = command.GradeLevel,
                Address = command.Address,
               EnrollmentDate = DateTime.UtcNow,
                GuardianFullName = command.GuardianFullName,
                GuardianContactNumber = command.GuardianContactNumber,
                StudentEmailAddress =  command.StudentEmailAddress
            };
            _dbcontext.StudentRegistrations.Add(entity);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return entity.id;

        }

    }
}
