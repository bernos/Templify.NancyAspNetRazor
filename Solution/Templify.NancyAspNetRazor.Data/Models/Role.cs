using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Templify.NancyAspNetRazor.Data.Models
{
    public class Role
    {
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new Collection<User>();
        }
    }
}
