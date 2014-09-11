using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using Bernos.Security;
using MediatR;
using Microsoft.Practices.ServiceLocation;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.ClientAppSettings;
using Nancy.CustomErrors;
using Nancy.Elmah;
using Nancy.Security;
using Templify.NancyAspNetRazor.Data;
using Templify.NancyAspNetRazor.Data.Commands;

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
            CustomErrors.Enable(pipelines);
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>()
            });
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(DataContext).Assembly, typeof(Bootstrapper).Assembly, typeof(Pbkdf2Sha1Configuration).Assembly)
                .Where(t => t != typeof(DataContext) && !t.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => new DataContext()).As<DbContext>();

            //builder.RegisterType<UserRepository>().As<IUserRepository>();

            var container = builder.Build();

            /* MEDIATR SETUP */
            var mediatrBuilder = new ContainerBuilder();
            var lazy = new Lazy<IServiceLocator>(() => new AutofacServiceLocator(container));
            var serviceLocatorProvider = new ServiceLocatorProvider(() => lazy.Value);

            mediatrBuilder.RegisterSource(new ContravariantRegistrationSource());
            mediatrBuilder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            mediatrBuilder.RegisterInstance(serviceLocatorProvider);



            mediatrBuilder.RegisterAssemblyTypes(typeof (DataContext).Assembly)
                .As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof (IRequestHandler<,>)))
                    .Select(i => new KeyedService("handler", i)));

            mediatrBuilder.RegisterGenericDecorator(typeof(LoggingDecorator<,>), typeof(IRequestHandler<,>),
                fromKey: "handler").Named("logging-handler", typeof(IRequestHandler<,>));

            mediatrBuilder.RegisterGenericDecorator(typeof (MediatorPipeline<,>), typeof (IRequestHandler<,>),
                fromKey: "logging-handler");

            mediatrBuilder.Update(container.ComponentRegistry);
            /* END MEDIATR SETUP */
            
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