using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.CustomErrors;
using Nancy.TinyIoc;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class ErrorHandlingConfiguration : CustomErrorsConfiguration
    {
        public ErrorHandlingConfiguration() : base()
        {
            ErrorViews[HttpStatusCode.NotFound] = "NotFound";
            ErrorViews[HttpStatusCode.InternalServerError] = "Error";
        }
    }
}