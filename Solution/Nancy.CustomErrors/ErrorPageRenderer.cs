using System;
using Nancy.Responses;
using Nancy.ViewEngines;

namespace Nancy.CustomErrors
{
    public class ErrorPageRenderer
    {
        public string ErrorView = "Error";
        public string NotFoundView = "Error";
        public string AuthorizationUrl = null;
        public Func<NancyContext, string> AuthorizationUrlBuilder = null; 

        public virtual void Render(NancyContext context, HttpStatusCode statusCode, IViewRenderer viewRenderer)
        {
            var error = context.Response as ErrorResponse;
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    if (!String.IsNullOrEmpty(AuthorizationUrl))
                    {
                        context.Response = new RedirectResponse(AuthorizationUrl);
                    }
                    else if (AuthorizationUrlBuilder != null)
                    {
                        context.Response = new RedirectResponse(AuthorizationUrlBuilder(context));
                    }
                    else
                    {
                        context.Response = viewRenderer.RenderView(context, ErrorView, new
                        {
                            Title = "Unauthorized",
                            Summary = error == null ? "You do not have permission to do that." : error.Error.Summary
                        }).WithStatusCode(statusCode);
                    }
                    break;
                case HttpStatusCode.Forbidden:
                    context.Response = viewRenderer.RenderView(context, ErrorView, new
                    {
                        Title = "Forbidden",
                        Summary = error == null ? "You do not have permission to do that." : error.Error.Summary
                    }).WithStatusCode(statusCode);
                    break;
                case HttpStatusCode.NotFound:
                    context.Response = viewRenderer.RenderView(context, NotFoundView, new
                    {
                        Title = "404 Not Found",
                        Summary = "Sorry, the resource you requested was not found."
                    }).WithStatusCode(statusCode); ;
                    break;
                case HttpStatusCode.InternalServerError:
                    context.Response = viewRenderer.RenderView(context, ErrorView, new
                    {
                        Title = "Sorry, something went wrong",
                        Summary = error == null ? "An unexpected error occurred." : error.Error.Summary,
                        Details = error == null ? null : error.Error.StackTrace
                    }).WithStatusCode(statusCode); ;
                    break;
            }
        }
    }
}