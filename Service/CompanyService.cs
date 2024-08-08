using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.ComponentModel.Design;

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

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyid, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyid, trackChanges);
        if(company == null) 
            throw new CompanyNotFoundException(companyid);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var entity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(entity);
        await _repository.SaveAsync();

        var companyDto = _mapper.Map<CompanyDto>(entity);
        
        return companyDto;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
    {
        if(companyIds is null)
            throw new IdParametersBadRequestException();

        var companies = await _repository.Company.GetByIdsAsync(companyIds, trackChanges);
        if(companies.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies)
    {
        if (companies is null)
            throw new CompanyCollectionBadRequest();

        var entities = _mapper.Map<IEnumerable<Company>>(companies);
        foreach (var entity in entities)
        {
            _repository.Company.CreateCompany(entity);
        }

        await _repository.SaveAsync();

        var entitiesDto = _mapper.Map<IEnumerable<CompanyDto>>(entities);
        var ids = string.Join(',', entitiesDto.Select(entitiesDto => entitiesDto.Id));

        return (entitiesDto, ids);
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);

        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateComapanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
        
        _mapper.Map(companyForUpdate, company);
        await _repository.SaveAsync();
    }
}
