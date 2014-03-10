using System;
using System.Linq;
using Nancy;
using Templify.NancyAspNetRazor.Data;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(Func<DataContext> dbFactory)
        {
            Get["/"] = parameters =>
            {
                var users = dbFactory().Users.ToList();

                return View["index", users];
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