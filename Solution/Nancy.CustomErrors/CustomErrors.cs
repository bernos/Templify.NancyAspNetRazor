using System;
using Nancy.Bootstrapper;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;

namespace Nancy.CustomErrors
{
    public class CustomErrors
    {
        public static CustomErrors Enable(IPipelines pipelines)
        {
            return Enable(pipelines, new JsonNetSerializer());
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

        private Func<Exception, Error> _exceptionParser = e => new Error
        {
            FullException = e.ToString(),
            Message = e.Message,
            StatusCode = HttpStatusCode.InternalServerError
        };
        

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