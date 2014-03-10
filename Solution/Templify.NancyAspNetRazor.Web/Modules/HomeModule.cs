using System;
using Nancy;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };

            Get["/api"] = parameters =>
            {
                return Response.AsJson(new
                {
                    Name = "My name",
                    Age = 32,
                    CreatedAt = DateTime.UtcNow
                });
            };
        }
    }
}