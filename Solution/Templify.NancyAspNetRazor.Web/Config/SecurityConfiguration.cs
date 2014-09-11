using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bernos.Security;
using Nancy.Bootstrapper;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class SecurityConfiguration : IRegistrations
    {
        private readonly ICollection<InstanceRegistration> _instanceRegistrations;

        public SecurityConfiguration()
        {
            _instanceRegistrations = new Collection<InstanceRegistration>
            {
                new InstanceRegistration(typeof(Pbkdf2Sha1Configuration), new Pbkdf2Sha1Configuration())
            };
        }

        public IEnumerable<TypeRegistration> TypeRegistrations { get { return null; } }
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations { get { return null; } }
        public IEnumerable<InstanceRegistration> InstanceRegistrations { get { return _instanceRegistrations; } }
    }
}