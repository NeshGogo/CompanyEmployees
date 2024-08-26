using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(p => p.company.Name).NotEmpty().MaximumLength(60);
        
        RuleFor(p => p.company.Address).NotEmpty().MaximumLength(60);
    }
}
