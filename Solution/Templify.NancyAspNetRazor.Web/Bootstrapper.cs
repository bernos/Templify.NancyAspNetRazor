using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.ClientAppSettings;
using Nancy.CustomErrors;
using Nancy.Elmah;
using Nancy.Security;
using Nancy.TinyIoc;
using Templify.NancyAspNetRazor.Data;

namespace Templify.NancyAspNetRazor.Web
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            ClientAppSettings.Enable(pipelines);
            Elmahlogging.Enable(pipelines, "elmah");
            CustomErrors.Enable(pipelines);
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>()
            });
        }
    }

    public class UserMapper : IUserMapper
    {
        private readonly Func<DataContext> _dbContextFactory;

        public UserMapper(Func<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            using (var db = _dbContextFactory())
            {
                var user = db.Users.SingleOrDefault(u => u.UserId == identifier);

                if (user == null)
                {
                    return null;
                }

                var roleClaims = user.Roles.SelectMany(r => r.Claims).Select(c => c.Name);
                var userClaims = user.Claims.Select(c => c.Name);

                return new UserIdentity(user.UserName, roleClaims.Concat(userClaims));
            }
        }
    }

    public class UserIdentity : IUserIdentity
    {
        private readonly string _username;
        private readonly IEnumerable<string> _claims;

        public UserIdentity(string username, IEnumerable<string> claims)
        {
            _username = username;
            _claims = claims;
        }

        public string UserName { get { return _username; } }
        public IEnumerable<string> Claims { get { return _claims; } }
    }

}