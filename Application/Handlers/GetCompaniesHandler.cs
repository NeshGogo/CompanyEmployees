using Application.Queries;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IRepositoryManager _repository;

    public GetCompaniesHandler(IRepositoryManager repository) => _repository = repository;

    public Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
