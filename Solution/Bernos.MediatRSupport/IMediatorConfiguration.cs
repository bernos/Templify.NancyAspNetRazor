using System;
using System.Collections.Generic;

namespace Bernos.MediatRSupport
{
    public interface IMediatorConfiguration
    {
        /// <summary>
        /// If true, CommonServiceLocator integration will automatically be set up
        /// </summary>
        bool RegisterCommonServiceLocator { get; }

        /// <summary>
        /// If true, all assemblies in the app domain will be scanned an all implementations of 
        /// IRequestHandler<,> will be automatically registered
        /// </summary>
        bool AutoRegisterRequestHandlerTypes { get; }

        /// <summary>
        /// Specific Request Handler implementations to register. Usually not required, if using
        /// AutoRegisterRequestHandlerTypes
        /// </summary>
        IEnumerable<Type> RequestHandlerRegistrations { get; } 

        /// <summary>
        /// Request handler decorator types to register
        /// </summary>
        IDictionary<string, Type> DecoratorRegistrations { get; }
    }
}