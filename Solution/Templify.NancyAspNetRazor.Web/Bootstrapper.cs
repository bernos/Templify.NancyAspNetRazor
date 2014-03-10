using Nancy.Conventions;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Templify.NancyAspNetRazor.Web.Config;

namespace Templify.NancyAspNetRazor.Web
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            JsonSerializerConfiguration.ConfigureApplicationContainer(container);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // Add scripts folder as static file folder
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", "Scripts"));
        }

        /// <summary>
        /// Create and configure our JsonSerializerConfiguration
        /// </summary>
        /// <param name="container"></param>
        /// <param name="overloads"></param>
        /// <returns></returns>
        private static JsonSerializer CreateJsonSerializer(TinyIoCContainer container, NamedParameterOverloads overloads)
        {
            var serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return serializer;
        }
    }


}