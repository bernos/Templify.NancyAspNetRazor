using MediatR;
using Nancy;
using Templify.NancyAspNetRazor.Data.Auth.Commands;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class SignupModule : NancyModule
    {
        public SignupModule(IMediator mediator)
        {
            Get["/signup"] = _ =>
            {
                var command = new RegisterUserCommand
                {
                    Password = "tony",
                    Username = "tony"
                };

                var result = mediator.Send(command);

                return Response.AsJson(result);
            };
        }
    }
}