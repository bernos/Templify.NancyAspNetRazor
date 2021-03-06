﻿using Bernos.Security;
using MediatR;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Templify.NancyAspNetRazor.Data;
using Templify.NancyAspNetRazor.Data.Auth.Commands;

namespace Templify.NancyAspNetRazor.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule(IMediator mediator)
        {
            Get["/login"]  = _ => View["login", new LoginCommand()];
            Get["/logout"] = _ => this.Logout("~/");

            Post["/login"] = _ =>
            {
                var model = this.Bind<LoginCommand>();

                if (model != null)
                {
                    var result = mediator.Send(model);

                    if (result.UserId.HasValue)
                    {
                        return this.Login(result.UserId.Value);
                    }
                }

                return View["login", model];
            };
        }
    }
}