using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanCompanyAPi.Services;

namespace CleanCompanyAPi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required");

            try
            {
                var result = await _companyService.SearchCompaniesAsync(query);
                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "External service error");
            }
        }

        [HttpGet("{orgnr}")]
        public async Task<IActionResult> GetByOrgNumber(string orgnr)
        {
            try
            {
                var company = await _companyService.GetByOrgNumberAsync(orgnr);

                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "External service error");
            }
        }
    }
}
