using System;
using Nancy;
using Nancy.Security;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule()
        {
            this.RequiresClaims(new string[] { Claim.IsAdministrator });

            Get["/admin"] = _ => View["admin"];

            Get["/claims"] = _ => View["claims"];
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