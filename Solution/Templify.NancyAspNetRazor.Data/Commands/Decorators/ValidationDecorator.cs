using Bernos.MediatRSupport;

namespace Templify.NancyAspNetRazor.Data.Commands.Decorators
{
    public class ValidationDecorator<TRequest> : IPreRequestHandler<TRequest>
    {
        public void Handle(TRequest request)
        {
            var a = "asdf";
        }
    }
}