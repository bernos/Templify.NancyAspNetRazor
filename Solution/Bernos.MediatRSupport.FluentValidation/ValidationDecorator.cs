using System.Linq;
using FluentValidation;

namespace Bernos.MediatRSupport.FluentValidation
{
    public class ValidationDecorator<TRequest> : IPreRequestHandler<TRequest>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidationDecorator(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public void Handle(TRequest request)
        {
            var context = new ValidationContext(request);

            var failures =
                _validators.Select(v => v.Validate(context)).SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }
    }
}