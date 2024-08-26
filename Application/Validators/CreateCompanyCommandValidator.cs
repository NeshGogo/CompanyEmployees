using Application.Commands;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Validators;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(p => p.company.Name).NotEmpty().MaximumLength(60);
        
        RuleFor(p => p.company.Address).NotEmpty().MaximumLength(60);
    }

    public override ValidationResult Validate(ValidationContext<CreateCompanyCommand> context)
    { 
        return context.InstanceToValidate.company is null 
            ? new ValidationResult([new ValidationFailure("CompanyForCreationDto", "CompanyForCreationDto object is null")])
            : base.Validate(context);
    }
}
