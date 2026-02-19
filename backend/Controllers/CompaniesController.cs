using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using CleanCompanyAPi.Services;

namespace CleanCompanyAPi.Controllers
=======
using CompanyApiClean.Models;
using System.Text.Json;

namespace CompanyApiClean.Controllers
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
{
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
<<<<<<< HEAD
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

=======
        private readonly HttpClient _httpClient;

        public CompaniesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET /api/companies?query=text
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required");

<<<<<<< HEAD
            try
            {
                var result = await _companyService.SearchCompaniesAsync(query);
                return Ok(result);
=======
            var url =
                $"https://data.brreg.no/enhetsregisteret/api/enheter?navn={Uri.EscapeDataString(query)}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, "External service error");

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                var list = new List<CompanyDto>();

                if (doc.RootElement.TryGetProperty("_embedded", out var embedded) &&
                    embedded.TryGetProperty("enheter", out var enheter))
                {
                    foreach (var e in enheter.EnumerateArray())
                    {
                        list.Add(MapCompany(e));
                    }
                }

                return Ok(list);
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
<<<<<<< HEAD
            catch
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

                if (company is null)
                    return NotFound();

=======
        }

        // GET /api/companies/{orgnr}
        [HttpGet("{orgnr}")]
        public async Task<IActionResult> GetByOrgNumber(string orgnr)
        {
            var url =
                $"https://data.brreg.no/enhetsregisteret/api/enheter/{orgnr}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();

                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, "External service error");

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                var company = MapCompany(doc.RootElement);
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
                return Ok(company);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
<<<<<<< HEAD
            catch
            {
                return StatusCode(502, "External service error");
            }
=======
        }

        // Mapper
        private static CompanyDto MapCompany(JsonElement e)
        {
            return new CompanyDto
            {
                OrganizationNumber =
                    e.GetProperty("organisasjonsnummer").GetString() ?? "",

                Name =
                    e.GetProperty("navn").GetString() ?? "",

                OrganizationForm =
                    e.GetProperty("organisasjonsform")
                     .GetProperty("beskrivelse")
                     .GetString() ?? "",

                Municipality =
                    e.TryGetProperty("forretningsadresse", out var addr) &&
                    addr.TryGetProperty("kommune", out var kommune)
                        ? kommune.GetString() ?? ""
                        : ""
            };
>>>>>>> 1bbcfe47eee900334074b6314ae64a563f03ca13
        }
    }
}
