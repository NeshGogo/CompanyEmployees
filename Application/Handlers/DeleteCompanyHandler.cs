using Application.Commands;
using Contracts;
using Entities.Exceptions;
using MediatR;

namespace Application.Handlers
{
    internal sealed class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, Unit>
    {
        private readonly IRepositoryManager _repository;

        public DeleteCompanyHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _repository.Company.GetCompanyAsync(request.Id, request.TrackChanges);

            if (company == null)
                throw new CompanyNotFoundException(request.Id);

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
