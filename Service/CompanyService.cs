using Contracts;
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
}
