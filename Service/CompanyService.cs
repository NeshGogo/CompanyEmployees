using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.Company.GetAllCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid companyid, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyid, trackChanges);
        if(company == null) 
            throw new CompanyNotFoundException(companyid);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var entity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(entity);
        _repository.Save();

        var companyDto = _mapper.Map<CompanyDto>(entity);
        
        return companyDto;
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if(companyIds is null)
            throw new IdParametersBadRequestException();

        var companies = _repository.Company.GetByIds(companyIds, trackChanges);
        if(companies.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null)
            throw new CompanyCollectionBadRequest();

        var entities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var entity in entities)
        {
            _repository.Company.CreateCompany(entity);
        }

        _repository.Save();

        var entitiesDto = _mapper.Map<IEnumerable<CompanyDto>>(entities);
        var ids = string.Join(',', entitiesDto.Select(entitiesDto => entitiesDto.Id));

        return (entitiesDto, ids);
    }
}
