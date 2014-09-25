using System;
using System.Threading.Tasks;
using log4net;
using MediatR;

namespace Bernos.MediatRSupport.log4net
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

    public class AsyncLoggingDecorator<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _innerHander;
        private readonly ILog _log;

        public AsyncLoggingDecorator(IAsyncRequestHandler<TRequest, TResponse> innerHandler, Func<Type, ILog> logFactory)
        {
            _innerHander = innerHandler;
            _log = logFactory(innerHandler.GetType());
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            _log.Info(string.Format("Request: {0}", message));
            var response = await _innerHander.Handle(message);
            _log.Info(string.Format("Response: {0}", response));

            return response;
        }
    }
}