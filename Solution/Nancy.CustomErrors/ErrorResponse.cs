using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Responses;

namespace Nancy.CustomErrors
{
    public class ErrorResponse : JsonResponse
    {
        public Error Error { get; set; }
        public ErrorResponse(Error error) : this(error, new DefaultJsonSerializer())
        {

        }

        public ErrorResponse(Error error, ISerializer serializer) : base(error, serializer)
        {
            StatusCode = error.StatusCode;
            Error = error;
        }
    }
}
