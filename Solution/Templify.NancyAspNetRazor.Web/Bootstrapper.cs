using Autofac;
using Bernos.Security;
using MediatR;
using MediatR.Extensions.Autofac;
using MediatR.Extensions.FluentValidation;
using MediatR.Extensions.log4net;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.ClientAppSettings;
using Nancy.CustomErrors;
using Nancy.Elmah;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Templify.NancyAspNetRazor.Data;
using Templify.NancyAspNetRazor.Web.Config;

namespace Templify.NancyAspNetRazor.Web
{
    using Nancy;

    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            ClientAppSettings.Enable(pipelines);
            Elmahlogging.Enable(pipelines, "elmah");
            CustomErrors.Enable(pipelines, new ErrorHandlingConfiguration());
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>()
            });
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(Bootstrapper).Assembly)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (Pbkdf2Sha1Configuration).Assembly)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(DataContext).Assembly)
                .Where(t => t != typeof(DataContext) && !t.GetInterfaces().Any(it => it.IsGenericType && (it.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || it.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>))))
                .Except<UnitOfWork>()
                .Except<DataContext>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => new DataContext()).As<DbContext>().InstancePerDependency();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();

            var container = builder.Build();

            var mediator = new AutofacMediatorBuilder(container)
                .WithRequestHandlerAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .WithRequestDecorator("logger",         typeof (LoggingRequestHandler<,>))
                .WithRequestDecorator("async-logger",   typeof (AsyncLoggingRequestHandler<,>))
                .WithRequestDecorator("validator",      typeof (ValidationRequestHandler<,>))
                .Build();
            
            return container;
        }

    }



    public class UserMapper : IUserMapper
    {
        private readonly IUserRepository _userRepository;

        public UserMapper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _userRepository.GetUser(identifier);

            if (user == null)
            {
                return null;
            }

            var allClaims = _userRepository.GetClaimsForUser(user.UserId);
            return new UserIdentity(user.UserName, allClaims);
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