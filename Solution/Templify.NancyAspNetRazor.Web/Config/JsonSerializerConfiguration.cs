using Nancy;
using Nancy.Bootstrapper;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class JsonSerializerConfiguration : IRegistrations
    {
        private readonly ICollection<TypeRegistration> _typeRegistrations;
 
        public JsonSerializerConfiguration()
        {
            _typeRegistrations = new Collection<TypeRegistration>
            {
                new TypeRegistration(typeof(ISerializer), typeof(CustomJsonSerializer), Lifetime.Singleton)
            };    
        }

        public IEnumerable<TypeRegistration> TypeRegistrations {
            get { return _typeRegistrations; }
        }

        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get { return null; } 
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get { return null; }
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