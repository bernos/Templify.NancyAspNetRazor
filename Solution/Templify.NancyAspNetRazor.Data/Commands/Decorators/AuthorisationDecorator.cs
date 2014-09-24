using Bernos.MediatRSupport;

namespace Templify.NancyAspNetRazor.Data.Commands.Decorators
{
    public interface IUserContext
    {
        string[] Claims { get; }
    }

    public class AuthorisationDecorator<TRequest> : IPreRequestHandler<TRequest>
    {
        private readonly IUserContext _userContext;
        public AuthorisationDecorator(IUserContext userContext)
        {
            _userContext = userContext;
        } 

        public void Handle(TRequest request)
        {
            var claims = _userContext.Claims;

            var a = "asf";
        }
    }

}