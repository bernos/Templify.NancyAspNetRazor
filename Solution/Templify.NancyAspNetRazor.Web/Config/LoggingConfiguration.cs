using log4net;
using log4net.Config;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public static class LoggingConfiguration
    {
        public static void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            XmlConfigurator.Configure();

            container.Register(typeof(ILog), (c, o) =>
            {
                return LogManager.GetLogger("root");
            });

            container.Register(typeof(Func<string, ILog>), (c, o) =>
            {
                return (Func<string, ILog>)(s => LogManager.GetLogger(s));
            });

            container.Register(typeof(Func<Type, ILog>), (c, o) =>
            {
                return (Func<Type, ILog>)(t => LogManager.GetLogger(t));
            });
        }            
    }
}