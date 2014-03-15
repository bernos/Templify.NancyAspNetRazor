using System;

namespace Nancy.CustomErrors
{
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string FullException { get; set; }
        public Error()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}