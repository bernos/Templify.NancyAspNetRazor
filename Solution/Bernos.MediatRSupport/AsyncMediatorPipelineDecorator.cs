using System.Threading.Tasks;
using MediatR;

namespace Bernos.MediatRSupport
{
    public class AsyncMediatorPipelineDecorator<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;

        public AsyncMediatorPipelineDecorator(
            IAsyncRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers
            )
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }

        public async Task<TResponse> Handle(TRequest message)
        {

            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(message);
            }

            var result = await _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}