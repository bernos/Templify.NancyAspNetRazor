using System;
using System.Data.Entity;
using System.Linq;
using Bernos.Security;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Templify.NancyAspNetRazor.Data;
using log4net;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(Func<Type, ILog> logger, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            var log = logger(typeof(HomeModule));

            Get["/"] = parameters =>
            {
                log.Info("Hello world");

               


                return View["index", null];
               
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