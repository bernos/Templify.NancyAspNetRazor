using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using Bernos.MediatRSupport.FluentValidation;
using Bernos.MediatRSupport.log4net;
using Bernos.Security;
using MediatR;
using MediatR.Extensions.Autofac;
using Microsoft.Practices.ServiceLocation;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.ClientAppSettings;
using Nancy.CustomErrors;
using Nancy.Elmah;
using Nancy.Security;
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

            builder.RegisterAssemblyTypes(typeof(DataContext).Assembly, typeof(Bootstrapper).Assembly, typeof(Pbkdf2Sha1Configuration).Assembly)
                .Where(t => t != typeof(DataContext) && !t.GetInterfaces().Any(it => it.IsGenericType && (it.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || it.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>))))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => new DataContext()).As<DbContext>();

    
            
            var container = builder.Build();

            var mediator = new AutofacMediatorBuilder(container)
                .WithRequestHandlerAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .WithRequestDecorator("logger", typeof (LoggingDecorator<,>))
                .WithRequestDecorator("async-logger", typeof (AsyncLoggingDecorator<,>))
                .WithRequestDecorator("validator", typeof (ValidationDecorator<,>))
                .Build();


            /*
            
            var mBuilder = new ContainerBuilder();
            var lazy = new Lazy<IServiceLocator>(() => new AutofacServiceLocator(container));
            var serviceLocatorProvider = new ServiceLocatorProvider(() => lazy.Value);

            mBuilder.RegisterSource(new ContravariantRegistrationSource());
            mBuilder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            mBuilder.RegisterInstance(serviceLocatorProvider);
           

            mBuilder.RegisterAssemblyTypes(typeof(LoginCommand).Assembly).As(t => t.GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                .Select(i => new KeyedService("handler", i)));

            mBuilder.RegisterGenericDecorator(typeof (LoggingDecorator<,>), typeof (IRequestHandler<,>),
                fromKey: "handler").Named("logger", typeof(IRequestHandler<,>));


            mBuilder.RegisterGenericDecorator(typeof(MediatorSupport.WrapperRequestHandler<,>), typeof(IRequestHandler<,>),
                fromKey: "logger");
            
            


            mBuilder.Update(container.ComponentRegistry);
            */
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