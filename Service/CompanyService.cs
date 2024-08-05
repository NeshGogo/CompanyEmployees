using Contracts;
using Entities.Models;
using Service.Contracts;

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

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            return companies;
        }
        catch (Exception ex)
        {
            _logger.LogError($"--> Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}
