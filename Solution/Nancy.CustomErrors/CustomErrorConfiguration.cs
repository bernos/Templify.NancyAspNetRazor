using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.CustomErrors
{
    public class CustomErrorConfiguration
    {
        public string ErrorView = "Error";
        public string NotFoundView = "Error";

        public Func<NancyContext, string> AuthorizationUrl = ctx => "/account/login"; 

        public Func<NancyContext, Exception, ISerializer, ErrorResponse> ResponseBuilder =
            (context, ex, serializer) =>
            {
                var error = new Error
                {
                    FullException = ex.ToString(),
                    Message = ex.Message
                };

                return new ErrorResponse(error, serializer).WithStatusCode(HttpStatusCode.InternalServerError) as ErrorResponse;
            };
    }
}
