using System;

namespace Nancy.CustomErrors
{
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Summary { get; set; }
        public string StackTrace { get; set; }
        public Error()
        {

        }
        public Error(Exception e)
        {
            Summary = e.Message;
            StackTrace = e.StackTrace;
        }
        public Error WithStatusCode(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
            return this;
        }
    }
}