using MediatR;
using Shared.DataTransferObjects;

namespace Application.Queries;

public sealed record class GetCompaniesQuery(bool TrackChanges) : IRequest<IEnumerable<CompanyDto>>;