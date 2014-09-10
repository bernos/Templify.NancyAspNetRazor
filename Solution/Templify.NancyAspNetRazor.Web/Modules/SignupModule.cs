using Nancy;
using Templify.NancyAspNetRazor.Data.Commands;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class SignupModule : NancyModule
    {
        public SignupModule(ICommandHandler<RegisterUserCommand, RegisterUserCommandResult> registerUserCommandHandler)
        {
            Get["/signup"] = _ =>
            {
                var result = registerUserCommandHandler.Execute(new RegisterUserCommand
                {
                    Username = "tony",
                    Password = "tony"
                });

                return View["signup"];
            };
        }
    }
}