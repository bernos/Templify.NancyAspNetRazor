using System;
using System.Reflection;
using MediatR;

namespace Bernos.MediatRSupport
{
    public interface IMediatorBuilder
    {
        IMediatorBuilder AddRequestDecorator(string name, Type decoratorType);
        IMediatorBuilder AddRequestHandlerAssemblies(params Assembly[] assemblies);
        IMediator Build();
    }
}