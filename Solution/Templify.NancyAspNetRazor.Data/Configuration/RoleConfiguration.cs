using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data.Configuration
{
    public class RoleConfiguration :EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(r => r.Name);
            HasMany(r => r.Claims).WithMany();
        }
    }
}