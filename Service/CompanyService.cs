using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public CompanyService(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _repository = repositoryManager;
        _logger = logger;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name, string.Join(' ', c.Address, c.Country)));
            return companiesDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"--> Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}
