using Bernos.Security;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Templify.NancyAspNetRazor.Data;
using Templify.NancyAspNetRazor.Data.Commands;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(ICommandHandler<LoginCommand, LoginCommandResult> loginCommand)
        {
            Get["/login"]  = _ => View["login", new LoginCommand()];
            Get["/logout"] = _ => this.Logout("~/");

            Post["/login"] = _ =>
            {
                var model = this.Bind<LoginCommand>();

                if (model != null && !string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password))
                {
                    var result = loginCommand.Execute(model);

                    if (result.UserId.HasValue)
                    {
                        return this.Login(result.UserId.Value);
                    }
                }

                return View["login", model];
            };
        }
    }
}