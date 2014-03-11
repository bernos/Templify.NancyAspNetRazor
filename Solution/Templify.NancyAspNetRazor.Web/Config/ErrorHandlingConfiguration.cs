using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using Nancy.ViewEngines;

// Based on http://paulstovell.com/blog/consistent-error-handling-with-nancy

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class ErrorHandlingConfiguration
    {
        public static ErrorHandlingConfiguration Enable(IPipelines pipelines, ISerializer serializer)
        {
            var config = new ErrorHandlingConfiguration(serializer);

            pipelines.OnError.AddItemToEndOfPipeline(config.HandleError);

            return config;
        }

        private readonly ISerializer _serializer;
        private Func<Exception, object> _exceptionParser;

        public ErrorHandlingConfiguration(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public ErrorHandlingConfiguration WithExceptionParser(Func<Exception, object> exceptionParser)
        {
            _exceptionParser = exceptionParser;
            return this;
        }

        public Response HandleError(NancyContext context, Exception e)
        {
            object error = new
            {
                Summary = e.Message,
                FullException = e.ToString()
            };

            if (_exceptionParser != null)
            {
                error = _exceptionParser(e);
            }
            
            return new JsonResponse(error, _serializer).WithStatusCode(HttpStatusCode.InternalServerError);
        }
    }

    public class ErrorStatusCodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        private readonly ISerializer _serializer;
        public ErrorStatusCodeHandler(IViewFactory viewFactory, ISerializer serializer)
            : base(viewFactory)
        {
            
            _serializer = serializer;
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
            if (ShouldRenderFriendlyErrorPage(context))
            {
                context.Response = RenderView(context, "Error").WithStatusCode(statusCode);
            }
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