using System;
using Nancy.Bootstrapper;
using Nancy.Responses;

namespace Nancy.CustomErrors
{
    public class CustomErrors
    {
        public static CustomErrors Enable(IPipelines pipelines)
        {
            return Enable(pipelines, new DefaultJsonSerializer());
        }

        public static CustomErrors Enable(IPipelines pipelines, ISerializer serializer)
        {
            var customErrors = new CustomErrors(serializer);

            pipelines.OnError.AddItemToEndOfPipeline(customErrors.HandleError);

            return customErrors;
        }

        private CustomErrors(ISerializer serializer)
        {
            _serializer = serializer;
        }

        private readonly ISerializer _serializer;

        private Func<Exception, Error> _exceptionParser = e => new Error(e).WithStatusCode(HttpStatusCode.InternalServerError);
        

        public CustomErrors WithExceptionParser(Func<Exception, Error> exceptionParser)
        {
            _exceptionParser = exceptionParser;
            return this;
        }

        private Response HandleError(NancyContext context, Exception e)
        {
            return new ErrorResponse(_exceptionParser(e), _serializer);
        }
    }
}