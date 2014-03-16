using System;
using System.Collections.Generic;

namespace Nancy.CustomErrors
{
    public interface IErrorConfiguration
    {
        IDictionary<HttpStatusCode, string> ErrorViews { get; set; }
        string GetAuthorizationUrl(NancyContext context);
        ErrorResponse HandleError(NancyContext context, Exception ex, ISerializer serializer);
    }
}
