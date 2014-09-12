using Bernos.MediatRSupport;

namespace Templify.NancyAspNetRazor.Data.Commands.Decorators
{
    public class AuthorisationDecorator<TRequest> : IPreRequestHandler<TRequest>
    {
        public void Handle(TRequest request)
        {
            var a = "asf";
        }
    }

}