using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCompanyAPi.Controllers
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
