using System;
using Nancy.Bootstrapper;
using Nancy.Responses;

namespace Nancy.CustomErrors
{
    public class CustomErrors
    {
        private static CustomErrorConfiguration _configuration;

        public static CustomErrorConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new CustomErrorConfiguration();
                }
                return _configuration;
            }
        }

        public static CustomErrors Enable(IPipelines pipelines, CustomErrorConfiguration configuration)
        {
            return Enable(pipelines, configuration, new DefaultJsonSerializer());
        }

        public static CustomErrors Enable(IPipelines pipelines, CustomErrorConfiguration configuration, ISerializer serializer)
        {
            _configuration = configuration;
            var customErrors = new CustomErrors(serializer);

            pipelines.OnError.AddItemToEndOfPipeline(customErrors.HandleError);

            return customErrors;
        }

        private CustomErrors(ISerializer serializer)
        {
            _serializer = serializer;
        }

        private readonly ISerializer _serializer;

        private Response HandleError(NancyContext context, Exception e)
        {
            return Configuration.ResponseBuilder(context, e, _serializer);
        }
    }
}