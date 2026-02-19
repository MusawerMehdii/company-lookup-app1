using CleanCompanyAPi.Models;

namespace CleanCompanyAPi.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> SearchCompaniesAsync(string query);
        Task<CompanyDto?> GetByOrgNumberAsync(string orgnr);
    }
}
