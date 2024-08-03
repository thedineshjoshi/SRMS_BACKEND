using Application.User.Commands.LoginUser;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<TokenResult>> Login([FromBody] LoginUserCommand loginuserCommand)
        {
            var result = await Mediator.Send(loginuserCommand);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
