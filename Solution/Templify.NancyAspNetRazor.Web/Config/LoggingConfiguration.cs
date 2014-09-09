using System.Collections.ObjectModel;
using log4net;
using log4net.Config;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class LoggingConfiguration : IRegistrations
    {
        private readonly ICollection<InstanceRegistration> _instanceRegistrations;  
        public LoggingConfiguration()
        {
            XmlConfigurator.Configure();

            _instanceRegistrations = new Collection<InstanceRegistration>
            {
                new InstanceRegistration(typeof(ILog), LogManager.GetLogger("root")),
                new InstanceRegistration(typeof(Func<string, ILog>), (Func<string, ILog>)(LogManager.GetLogger)),
                new InstanceRegistration(typeof(Func<Type, ILog>), (Func<Type, ILog>)(LogManager.GetLogger))
            };
        }

        public IEnumerable<TypeRegistration> TypeRegistrations {
            get { return null; }
        }
        
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations {
            get { return null; } 
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations {
            get { return _instanceRegistrations; }
        }
    }
}