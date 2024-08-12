using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<EmployeeDto> dataShapper)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repository, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repository, logger, mapper, dataShapper));
    }

    public ICompanyService CompanyService => _companyService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
}
