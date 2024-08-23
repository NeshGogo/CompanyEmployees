using MediatR;
using Shared.DataTransferObjects;

namespace Application.Commands;

public  sealed record CreateCompanyCommand(CompanyForCreationDto company) : IRequest<CompanyDto>;