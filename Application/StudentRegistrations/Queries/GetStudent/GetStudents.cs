using Application.Common.Interfaces;
using Application.Common.Mapping;
using Application.Common.Model;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.StudentRegistrations.Queries.GetStudent
{
    public class GetStudents : IRequest<PaginationList<StudentVm>>
    {
        public  int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetStudentsHandler : IRequestHandler<GetStudents, PaginationList<StudentVm>>
    {
        private readonly IApplicationDbContext _context;
        public GetStudentsHandler(IApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<PaginationList<StudentVm>> Handle (GetStudents request , CancellationToken cancellationToken)
        {
            var items = await _context.StudentRegistrations.Select(x => new StudentVm
            {
                id = x.id,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                GradeLevel = x.GradeLevel,
                Address = x.Address,
                GuardianFullName = x.GuardianFullName,
                GuardianContactNumber = x.GuardianContactNumber,
                StudentEmailAddress = x.StudentEmailAddress,
                EnrollmentDate = x.EnrollmentDate
            }).PaginationListAsync(request.PageNumber,request.PageSize);
            return items;
        }
    }
}
