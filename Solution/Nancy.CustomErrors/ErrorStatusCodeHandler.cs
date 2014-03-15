using System.Linq;
using System.Net;
using Nancy.ErrorHandling;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.ViewEngines;

namespace Nancy.CustomErrors
{
    public class ErrorStatusCodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        private readonly ISerializer _serializer;
        private readonly ErrorPageRenderer _renderer;

        public ErrorStatusCodeHandler(IViewFactory viewFactory, ErrorPageRenderer renderer)
            : this(viewFactory, new DefaultJsonSerializer(), renderer)
        {
        }

        public ErrorStatusCodeHandler(IViewFactory viewFactory, ISerializer serializer, ErrorPageRenderer renderer)
            : base(viewFactory)
        {
            _serializer = serializer;
            _renderer = renderer;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound
                   || statusCode == HttpStatusCode.InternalServerError
                   || statusCode == HttpStatusCode.Forbidden
                   || statusCode == HttpStatusCode.Unauthorized;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var clientWantsHtml = ShouldRenderFriendlyErrorPage(context);
            
            if (!clientWantsHtml)
            {
                if (context.Response is NotFoundResponse)
                {
                    // Normally we return 404's ourselves so we have an ErrorResponse. 
                    // But if no route is matched, Nancy will set a NotFound response itself. 
                    // When this happens we still want to return our nice JSON response.
                    context.Response = new ErrorResponse(new Error
                    {
                        Message = "The requested resource could not be found"
                    }, _serializer).WithStatusCode(statusCode);
                }

                // Pass the existing response through
                return;
            }

            _renderer.Render(context, statusCode, this);
        }

        private static bool ShouldRenderFriendlyErrorPage(NancyContext context)
        {
            var ranges =
                context.Request.Headers.Accept.OrderByDescending(o => o.Item2)
                    .Select(o => MediaRange.FromString(o.Item1))
                    .ToList();

            foreach (var range in ranges)
            {
                if (range.Matches("application/json"))
                {
                    return false;
                }

                if (range.Matches("text/json"))
                {
                    return false;
                }

                if (range.Matches("text/html"))
                {
                    return true;
                }
            }

            return true;
        }
    }
}