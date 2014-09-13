using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using MediatR;
using Microsoft.Practices.ServiceLocation;

namespace Bernos.MediatRSupport.Autofac
{
    public static class MediatorSupport
    {
        public static void Enable(ILifetimeScope container, IMediatorConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            var lazy = new Lazy<IServiceLocator>(() => new AutofacServiceLocator(container));
            var serviceLocatorProvider = new ServiceLocatorProvider(() => lazy.Value);
            var key = "handler";

            //builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterInstance(serviceLocatorProvider);

            if (configuration.AutoRegisterAssemblyRequestHandlerTypes)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                builder.RegisterAssemblyTypes(assemblies).As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof (IRequestHandler<,>)))
                    .Select(i => new KeyedService(key, i)));
            }

            if (configuration.RequestHandlerRegistrations != null)
            {
                builder.RegisterTypes(configuration.RequestHandlerRegistrations.ToArray()).As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(i => new KeyedService(key, i)));
            }

            if (configuration.DecoratorRegistrations != null)
            {
                foreach (var decoratorRegistration in configuration.DecoratorRegistrations)
                {
                    builder.RegisterGenericDecorator(decoratorRegistration.Value, typeof(IRequestHandler<,>),
                        fromKey: key).Named(decoratorRegistration.Key, typeof(IRequestHandler<,>));

                    key = decoratorRegistration.Key;
                }
            }

            // Register pipeline
            builder.RegisterGenericDecorator(typeof(MediatorPipelineDecorator<,>), typeof(IRequestHandler<,>),
                fromKey: key);

            if (configuration.PreRequestRegistrations != null)
            {
                foreach (var preRequestRegistration in configuration.PreRequestRegistrations)
                {
                    builder.RegisterGeneric(preRequestRegistration).As(typeof (IPreRequestHandler<>));
                }
            }

            if (configuration.PostRequestRegistrations != null)
            {
                foreach (var postRequestRegistration in configuration.PostRequestRegistrations)
                {
                    builder.RegisterGeneric(postRequestRegistration).As(typeof (IPostRequestHandler<,>));
                }
            }
            
            builder.Update(container.ComponentRegistry);
        }
    }
}