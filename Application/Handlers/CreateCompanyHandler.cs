using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Models;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public CreateCompanyHandler(IRepositoryManager repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = _mapper.Map<Company>(request.company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();

        var companyDto = _mapper.Map<CompanyDto>(companyEntity);

        return companyDto;
    }
}
