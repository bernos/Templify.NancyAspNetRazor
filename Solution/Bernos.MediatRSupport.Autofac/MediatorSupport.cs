using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using MediatR;
using Microsoft.Practices.ServiceLocation;

namespace Bernos.MediatRSupport.Autofac
{
    public static class MediatorSupport
    {
        private const string HandlerKey = "handler";
        public static void Enable(ILifetimeScope container, IMediatorConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            if (configuration.RegisterCommonServiceLocator)
            {
                RegisterCommonServiceLocator(container);
            }

            if (configuration.AutoRegisterRequestHandlerTypes)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                builder.RegisterAssemblyTypes(assemblies).As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof (IRequestHandler<,>)))
                    .Select(i => new KeyedService(HandlerKey, i)));
            }

            if (configuration.RequestHandlerRegistrations != null)
            {
                builder.RegisterTypes(configuration.RequestHandlerRegistrations.ToArray()).As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(i => new KeyedService(HandlerKey, i)));
            }

            var key = HandlerKey;
            
            if (configuration.DecoratorRegistrations != null)
            {

                foreach (var decoratorRegistration in configuration.DecoratorRegistrations)
                {
                    builder.RegisterGenericDecorator(decoratorRegistration.Value, typeof(IRequestHandler<,>),
                        fromKey: key).Named(decoratorRegistration.Key, typeof(IRequestHandler<,>));

                    key = decoratorRegistration.Key;
                }
            }

            builder.RegisterGenericDecorator(typeof(WrapperRequestHandler<,>), typeof(IRequestHandler<,>),
                fromKey: key);
           
            builder.Update(container.ComponentRegistry);
        }

        private static void RegisterCommonServiceLocator(ILifetimeScope container)
        {
            var builder = new ContainerBuilder();
            var lazy = new Lazy<IServiceLocator>(() => new AutofacServiceLocator(container));
            var serviceLocatorProvider = new ServiceLocatorProvider(() => lazy.Value);

            builder.RegisterInstance(serviceLocatorProvider);
            builder.Update(container.ComponentRegistry);
        }

        public class WrapperRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            private readonly IRequestHandler<TRequest, TResponse> _innerHandler;

            public WrapperRequestHandler(IRequestHandler<TRequest, TResponse> innerHandler)
            {
                _innerHandler = innerHandler;
            }

            public TResponse Handle(TRequest message)
            {
                return _innerHandler.Handle(message);
            }
        } 
    }
}