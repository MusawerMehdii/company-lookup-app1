using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

<<<<<<< HEAD
namespace CleanCompanyAPi.Controllers
=======
namespace CompanyApiClean.Controllers
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
{
    [ApiController]
    [Route("api/me")]
    [Authorize]
    public class MeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                authenticated = User.Identity?.IsAuthenticated,
                claims = User.Claims.Select(c => new
                {
                    type = c.Type,
                    value = c.Value
                })
            });
        }
    }
}
