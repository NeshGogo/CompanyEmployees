using MediatR;

namespace Application.Commands;

public sealed record DeleteCompanyCommand(Guid Id, bool TrackChanges) : IRequest<Unit>;