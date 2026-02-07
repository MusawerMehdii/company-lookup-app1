using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanyApiClean.Models;
using System.Text.Json;

namespace CompanyApiClean.Controllers
{
    [ApiController]
    [Route("api/companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CompaniesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET /api/companies?query=text
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required");

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
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
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
                return Ok(company);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "External service timeout");
            }
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
        }
    }
}
