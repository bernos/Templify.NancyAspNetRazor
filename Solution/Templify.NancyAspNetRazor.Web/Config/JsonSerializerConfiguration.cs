using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Serialization.JsonNet;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public static class JsonSerializerConfiguration
    {
        public static void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<ISerializer, CustomJsonSerializer>().AsSingleton();
        }
    }

    public class CustomJsonSerializer : JsonNetSerializer
    {
        public CustomJsonSerializer() : base(new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        })
        {
            
        }
    }
}