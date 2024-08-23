﻿using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;

namespace Application.Handlers;

internal sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateCompanyHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = await _repository.Company.GetCompanyAsync(request.Id, request.TrackChanges);
        
        if (companyEntity == null)
            throw new CompanyNotFoundException(request.Id);

        _mapper.Map(request.Company, companyEntity);
        await _repository.SaveAsync();

        return Unit.Value;
    }
}
