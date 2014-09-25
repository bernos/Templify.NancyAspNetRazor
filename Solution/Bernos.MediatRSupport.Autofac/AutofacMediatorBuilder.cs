using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using MediatR;
using Microsoft.Practices.ServiceLocation;

namespace Bernos.MediatRSupport.Autofac
{
    public class AutofacMediatorBuilder : IMediatorBuilder
    {
        private const string HandlerKey = "handler";
        private const string AsyncHandlerKey = "async-handler";
        private readonly ILifetimeScope _container;
        private readonly ContainerBuilder _builder;
        private string _key;
        private string _asyncKey;
        private bool _isBuilt;
        public AutofacMediatorBuilder(ILifetimeScope container)
        {
            _key = HandlerKey;
            _asyncKey = AsyncHandlerKey;
            _container = container;
            _builder = new ContainerBuilder();
        }
        
        public IMediatorBuilder AddRequestDecorator(string name, Type decoratorType)
        {
            if (_isBuilt)
            {
                throw new Exception("Cannot call AddRequestDecorator after Build() has been called");
            }

            if (decoratorType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>)))
            {
                _builder.RegisterGenericDecorator(decoratorType, typeof(IAsyncRequestHandler<,>),
                fromKey: _asyncKey).Named(name, typeof(IAsyncRequestHandler<,>));

                _asyncKey = name;
            }
            else if (
                decoratorType.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IRequestHandler<,>)))
            {
                _builder.RegisterGenericDecorator(decoratorType, typeof (IRequestHandler<,>),
                    fromKey: _key).Named(name, typeof (IRequestHandler<,>));

                _key = name;
            }
            else
            {
                throw new ArgumentException("Decorator type must implement IRequestHandler<TRequest,TResponse> or IAsyncRequestHandler<TRequest, TResponse>", "decoratorType");
            }

            return this;
        }

        public IMediatorBuilder AddRequestHandlerAssemblies(params Assembly[] assemblies)
        {
            _builder.RegisterAssemblyTypes(assemblies).As(t => t.GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                .Select(i => new KeyedService(HandlerKey, i)));

            _builder.RegisterAssemblyTypes(assemblies).As(t => t.GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>)))
                .Select(i => new KeyedService(AsyncHandlerKey, i)));

            return this;
        }

        public IMediator Build()
        {
            if (_isBuilt)
            {
                throw new Exception("Build() can only be called once");
            }

            _builder.RegisterSource(new ContravariantRegistrationSource());
            _builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            var lazy = new Lazy<IServiceLocator>(() => new AutofacServiceLocator(_container));
            var serviceLocatorProvider = new ServiceLocatorProvider(() => lazy.Value);

            _builder.RegisterInstance(serviceLocatorProvider);
            
            _builder.RegisterGenericDecorator(typeof(WrapperRequestHandler<,>), typeof(IRequestHandler<,>),
                fromKey: _key);

            _builder.RegisterGenericDecorator(typeof(AsyncWrapperRequestHandler<,>), typeof(IAsyncRequestHandler<,>),
                fromKey: _asyncKey);

            _builder.Update(_container.ComponentRegistry);

            _isBuilt = true;

            return serviceLocatorProvider().GetInstance<IMediator>();
        }
    }

    internal class WrapperRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
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

    internal class AsyncWrapperRequestHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _innerHandler;

        public AsyncWrapperRequestHandler(IAsyncRequestHandler<TRequest, TResponse> innerHandler)
        {
            _innerHandler = innerHandler;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            return await _innerHandler.Handle(message);
        }
    } 
}