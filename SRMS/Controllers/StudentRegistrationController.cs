using Application.Common.Interfaces;
using Application.Common.Model;
using Application.StudentRegistrations.Commands.CreateStudent;
using Application.StudentRegistrations.Queries.GetStudent;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SRMS.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "CanAddStudents")]
    [ApiController]
    public class StudentRegistrationController : ApiControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
    public StudentRegistrationController (IApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        // GET: api/<StudentRegistrationController>
        [HttpGet]
        public async Task<PaginationList<StudentVm>> Get([FromQuery] GetStudents query)
        {
            var result = await Mediator.Send(query);
            return result;
        }

        // GET api/<StudentRegistrationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentRegistrationController>
        
        [HttpPost]
        public async Task<Guid> Post([FromBody] CreateStudentCommand command,CancellationToken cancellationToken)
        {
            return await Mediator.Send(command, cancellationToken);

        }

        // PUT api/<StudentRegistrationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentRegistrationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
