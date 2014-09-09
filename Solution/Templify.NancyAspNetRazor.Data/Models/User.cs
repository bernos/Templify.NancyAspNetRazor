using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Templify.NancyAspNetRazor.Data.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Claim> Claims { get; set; } 
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            Roles = new Collection<Role>();
            Claims = new Collection<Claim>();
        }
    }

}
