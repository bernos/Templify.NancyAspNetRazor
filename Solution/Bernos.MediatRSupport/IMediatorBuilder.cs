using System;
using MediatR;

namespace Bernos.MediatRSupport
{
    public interface IMediatorBuilder
    {
        IMediatorBuilder AddRequestDecorator(string name, Type decoratorType);
        IMediator Build();
    }
}