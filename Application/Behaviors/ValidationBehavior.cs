using Entities.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => 
        _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()) 
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var errorDictionary = _validators
            .Select(x => x.Validate(context))
            .SelectMany(p => p.Errors)
            .Where(p => p != null)
            .GroupBy(p =>
                p.PropertyName.Substring(p.PropertyName.IndexOf('.') + 1),
                p => p.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(p => p.Key, p => p.Values);

        if(errorDictionary.Any())
            throw new ValidationAppException(errorDictionary);

        return await next();
    }
}
