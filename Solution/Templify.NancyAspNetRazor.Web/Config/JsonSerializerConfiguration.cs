using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public static class JsonSerializerConfiguration
    {
        public static void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(typeof (JsonSerializer), (c, options) => new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}