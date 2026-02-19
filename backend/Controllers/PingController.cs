using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

<<<<<<< HEAD
namespace CleanCompanyAPi.Controllers
=======
namespace CompanyApiClean.Controllers
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("Public endpoint works");
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok("Secure endpoint works");
        }
    }
}
