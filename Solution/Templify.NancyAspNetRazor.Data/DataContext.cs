using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data
{
    public class DataContext : DbContext
    {
        public IDbSet<User> Users { get; set; } 
    }
}
