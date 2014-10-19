using System;
using System.Collections.Generic;
using System.Web;
using Elmah;
using Nancy;
using Nancy.Security;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule()
        {
            //this.RequiresClaims(new string[] { Claim.IsAdministrator });

            Get["/admin"] = _ => View["admin"];

            Get["/claims"] = _ => View["claims"];

            Get["/errors"] = _ =>
            {
                var log = ErrorLog.GetDefault(HttpContext.Current);

                var errors = new List<ErrorLogEntry>();
                var totalErrors = log.GetErrors(0, 5, errors);


                return 200;
            };
            /*
            Post["/claims"] = _ =>
            {

            };

            Delete["/claims"] = _ =>
            {

            };*/
        }
    }
}