using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.CustomErrors;
using Nancy.TinyIoc;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public static class ErrorHandlingConfiguration
    {
        public static void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IErrorConfiguration, CustomErrorConfiguration>().AsSingleton();
        }
    }

    public class CustomErrorConfiguration : DefaultErrorConfiguration
    {
        public CustomErrorConfiguration() : base()
        {
            ErrorViews[HttpStatusCode.NotFound] = "NotFound";
            ErrorViews[HttpStatusCode.InternalServerError] = "Error";
        }
    }
}