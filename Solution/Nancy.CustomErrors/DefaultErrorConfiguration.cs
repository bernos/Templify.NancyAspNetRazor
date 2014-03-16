using System;
using System.Collections.Generic;

namespace Nancy.CustomErrors
{
    public class DefaultErrorConfiguration : IErrorConfiguration
    {
        public virtual string GetAuthorizationUrl(NancyContext context)
        {
            return "/account/login";
        }

        public virtual ErrorResponse HandleError(NancyContext context, Exception ex, ISerializer serializer)
        {
            var error = new Error
            {
                FullException = ex.ToString(),
                Message = ex.Message
            };

            return new ErrorResponse(error, serializer).WithStatusCode(HttpStatusCode.InternalServerError) as ErrorResponse;
        }

        public IDictionary<HttpStatusCode, string> ErrorViews { get; set; }

        public DefaultErrorConfiguration()
        {
            ErrorViews = new Dictionary<HttpStatusCode, string>
            {
                { HttpStatusCode.NotFound,              "Error" },
                { HttpStatusCode.InternalServerError,   "Error" },
                { HttpStatusCode.Forbidden,             "Error" }
            };
        }
    }
}
