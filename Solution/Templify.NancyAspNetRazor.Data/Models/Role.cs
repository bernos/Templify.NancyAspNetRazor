using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Templify.NancyAspNetRazor.Data.Models
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
