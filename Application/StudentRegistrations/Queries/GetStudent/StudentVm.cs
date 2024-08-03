using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StudentRegistrations.Queries.GetStudent
{
    public class StudentVm()
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string GradeLevel { get; set; }
        public string Address { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string GuardianFullName { get; set; }
        public int GuardianContactNumber { get; set; }
        public string StudentEmailAddress { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
