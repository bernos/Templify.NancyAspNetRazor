using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bernos.MediatRSupport
{
    public class DefaultMediatorConfiguration : IMediatorConfiguration
    {
        private readonly IDictionary<string, Type> _decoratorRegistrations;
        private readonly ICollection<Type> _preRequestRegistrations;
        private readonly ICollection<Type> _postRequestRegistrations;
        private readonly ICollection<Type> _requestHandlerRegistrations;
        
        public DefaultMediatorConfiguration()
        {
            _decoratorRegistrations = new Dictionary<string, Type>();
            _preRequestRegistrations = new Collection<Type>();
            _postRequestRegistrations = new Collection<Type>();
            _requestHandlerRegistrations = new Collection<Type>();

            RegisterCommonServiceLocator = true;
            AutoRegisterRequestHandlerTypes = true;

            Configure();
        }

        protected void AddRequestHandler(Type requestHandlerType)
        {
            _requestHandlerRegistrations.Add(requestHandlerType);
        }

        protected void AddPreRequestHandler(Type preRequestHandler)
        {
            _preRequestRegistrations.Add(preRequestHandler);
        }

        protected void AddPostRequestHandler(Type postRequestHandler)
        {
            _postRequestRegistrations.Add(postRequestHandler);
        }

        protected void AddRequestDecorator(string name, Type decoratorType)
        {
            _decoratorRegistrations.Add(name, decoratorType);
        }

        protected virtual void Configure()
        {
            
        }

        public bool RegisterCommonServiceLocator { get; private set; }
        public bool AutoRegisterRequestHandlerTypes { get; private set; }
        public IEnumerable<Type> RequestHandlerRegistrations { get { return _requestHandlerRegistrations; } }
        public IDictionary<string, Type> DecoratorRegistrations { get { return _decoratorRegistrations; } }
        public IEnumerable<Type> PreRequestRegistrations { get { return _preRequestRegistrations; } }
        public IEnumerable<Type> PostRequestRegistrations { get { return _postRequestRegistrations; } }
    }
}