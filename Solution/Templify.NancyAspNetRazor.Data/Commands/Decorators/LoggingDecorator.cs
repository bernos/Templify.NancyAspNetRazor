using System;
using log4net;
using MediatR;

namespace Templify.NancyAspNetRazor.Data.Commands.Decorators
{
    public class LoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHander;
        private readonly ILog _log;

        public LoggingDecorator(IRequestHandler<TRequest, TResponse> innerHandler, Func<Type, ILog> logFactory)
        {
            _innerHander = innerHandler;
            _log = logFactory(innerHandler.GetType());
        }

        public TResponse Handle(TRequest message)
        {
            _log.Info(string.Format("Request: {0}", message));
            var response =  _innerHander.Handle(message);
            _log.Info(string.Format("Response: {0}", response));

            return response;
        }
    }


    
}