using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.CustomErrors;
using Nancy.TinyIoc;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class CustomErrorConfiguration : CustomErrorsConfiguration
    {
        public CustomErrorConfiguration() : base()
        {
            ErrorViews[HttpStatusCode.NotFound] = "NotFound";
            ErrorViews[HttpStatusCode.InternalServerError] = "Error";
        }
    }
}