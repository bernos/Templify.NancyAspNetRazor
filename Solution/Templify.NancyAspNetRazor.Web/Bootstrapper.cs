using Nancy.Bootstrapper;
using Nancy.ClientAppSettings;
using Nancy.Conventions;
using Nancy.CustomErrors;
using Nancy.Elmah;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
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

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            ClientAppSettings.Enable(pipelines);
            Elmahlogging.Enable(pipelines, "elmah");

            CustomErrors.Enable(pipelines);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
            JsonSerializerConfiguration.ConfigureApplicationContainer(container);
            //LoggingConfiguration.ConfigureApplicationContainer(container);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // Add scripts folder as static file folder
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", "Scripts"));
        }        
    }
}