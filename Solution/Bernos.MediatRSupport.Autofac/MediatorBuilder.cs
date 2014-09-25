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
    public class MediatorBuilder : IMediatorBuilder
    {
        private const string HandlerKey = "handler";
        private readonly ILifetimeScope _container;
        private readonly ContainerBuilder _builder;
        private string _key;
        private bool _isBuilt;
        public MediatorBuilder(ILifetimeScope container)
        {
            _key = HandlerKey;
            _container = container;
            _builder = new ContainerBuilder();
        }

        public IMediatorBuilder AddRequestDecorator(string name, Type decoratorType)
        {
            if (_isBuilt)
            {
                throw new Exception("Cannot call AddRequestDecorator after Build() has been called");
            }

            _builder.RegisterGenericDecorator(decoratorType, typeof(IRequestHandler<,>),
                fromKey: _key).Named(name, typeof(IRequestHandler<,>));

            _key = name;

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


            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            _builder.RegisterAssemblyTypes(assemblies).As(t => t.GetInterfaces()
                .Where(i => TypeExtensions.IsClosedTypeOf((Type) i, typeof(IRequestHandler<,>)))
                .Select(i => new KeyedService(HandlerKey, i)));

            _builder.RegisterGenericDecorator(typeof(MediatorSupport.WrapperRequestHandler<,>), typeof(IRequestHandler<,>),
                fromKey: _key);

            _builder.Update(_container.ComponentRegistry);

            _isBuilt = true;

            return serviceLocatorProvider().GetInstance<IMediator>();
        }
    }
}