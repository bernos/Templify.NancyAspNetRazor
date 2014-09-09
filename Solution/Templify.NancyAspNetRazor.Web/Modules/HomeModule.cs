using System;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Templify.NancyAspNetRazor.Data;
using log4net;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(Func<DataContext> dbFactory, Func<Type, ILog> logger)
        {
            var log = logger(typeof(HomeModule));

            Get["/"] = parameters =>
            {
                log.Info("Hello world");
                var users = dbFactory().Users.ToList();

                this.Login(users.First().UserId);
                

                return View["index", users];
            };

            Get["/error"] = parameters =>
            {
                throw new Exception("This is an example error.");
            };

            Get["/forbidden"] = parameters =>
            {
                return HttpStatusCode.Forbidden;
            };

            Get["/private"] = parameters =>
            {
                return HttpStatusCode.Unauthorized;
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