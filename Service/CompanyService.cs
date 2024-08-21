using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Responses;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _repository = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public ApiBaseResponse GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.Company.GetAllCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
    }

    public ApiBaseResponse GetCompany(Guid companyid, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyid, trackChanges);
        if(company == null) 
            return new CompanyNotFoundResponse(companyid);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return new ApiOkResponse<CompanyDto>(companyDto);
    }
}
