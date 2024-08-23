﻿using Application.Queries;
using AutoMapper;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetCompaniesHandler(IRepositoryManager repository, IMapper mapper) 
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(request.TrackChanges);

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
       
        return companiesDto;
    }
}
