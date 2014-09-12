using System;
using System.Collections.Generic;

namespace Bernos.MediatRSupport
{
    public interface IMediatorConfiguration
    {
        bool AutoRegisterAssemblyRequestHandlerTypes { get; }
        IEnumerable<Type> RequestHandlerRegistrations { get; } 
        IDictionary<string, Type> DecoratorRegistrations { get; }
        IEnumerable<Type> PreRequestRegistrations { get; }
        IEnumerable<Type> PostRequestRegistrations { get; }
    }
}