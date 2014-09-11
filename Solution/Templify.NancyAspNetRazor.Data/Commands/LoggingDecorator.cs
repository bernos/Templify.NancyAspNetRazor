using System;
using log4net;
using MediatR;

namespace Templify.NancyAspNetRazor.Data.Commands
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

    public class PreRequestHandler<TRequest> : IPreRequestHandler<TRequest>
    {
        public void Handle(TRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }

    public interface IPostRequestHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }

    public class MediatorPipeline<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {

        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;

        public MediatorPipeline(
            IRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers
            )
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }

        public TResponse Handle(TRequest message)
        {

            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(message);
            }

            var result = _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}