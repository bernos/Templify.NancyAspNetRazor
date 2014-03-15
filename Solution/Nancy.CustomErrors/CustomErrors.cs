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

        private Func<NancyContext, Exception, ISerializer, ErrorResponse> _responseBuilder =
            (context, e, serializer) =>
            {
                var error = new Error
                {
                    FullException = e.ToString(),
                    Message = e.Message
                };

                return new ErrorResponse(error, serializer).WithStatusCode(HttpStatusCode.InternalServerError) as ErrorResponse;
            };

        public CustomErrors WithResponseBuilder(Func<NancyContext, Exception, ISerializer, ErrorResponse> responseBuilder)
        {
            _responseBuilder = responseBuilder;
            return this;
        }

        private Response HandleError(NancyContext context, Exception e)
        {
            return _responseBuilder(context, e, _serializer);
        }
    }
}