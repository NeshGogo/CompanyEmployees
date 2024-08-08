using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
    Task UpdateComapanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
}
