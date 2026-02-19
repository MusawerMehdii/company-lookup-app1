using System.Text.Json;
using CleanCompanyAPi.Models;

namespace CleanCompanyAPi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;

        public CompanyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<CompanyDto>> SearchCompaniesAsync(string query)
        {
            var url =
                $"https://data.brreg.no/enhetsregisteret/api/enheter?navn={Uri.EscapeDataString(query)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("External service error");

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

            return list;
        }

        public async Task<CompanyDto?> GetByOrgNumberAsync(string orgnr)
        {
            var url =
                $"https://data.brreg.no/enhetsregisteret/api/enheter/{orgnr}";

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("External service error");

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            return MapCompany(doc.RootElement);
        }

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
