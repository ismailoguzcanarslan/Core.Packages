using Core.CrossCuttingConcerns.Exceptions.Types;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Core.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace Core.Application.PipeLines.Validation;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly IEnumerable<IValidator<TRequest>> _validator;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);

        IEnumerable<ValidationExceptionModel> errors = _validator
            .Select(validator=>validator.Validate(context))
            .SelectMany(result=>result.Errors)
            .Where(failure => failure!=null)
            .GroupBy(
                keySelector: p=>p.PropertyName,
                resultSelector: (propertName, errors)=> new ValidationExceptionModel { Property = propertName, Errors = errors.Select(e=>e.ErrorMessage)}
            ).ToList();

        if(errors.Any())
            throw new ValidationException(errors);
        TResponse response = await next();
        return response;
    }
}
