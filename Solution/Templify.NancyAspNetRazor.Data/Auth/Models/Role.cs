using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Templify.NancyAspNetRazor.Data.Auth.Models
{
    public class Role
    {
        public const string Administrator = "Administrator";

        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Claim> Claims { get; set; } 

        public Role()
        {
            Users = new Collection<User>();
            Claims = new Collection<Claim>();
        }
    }
}
