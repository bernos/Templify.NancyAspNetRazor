﻿using System.Data.Entity.ModelConfiguration;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Data.Configuration
{
    public class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public ClaimConfiguration()
        {
            HasKey(c => c.Name);
        }
    }
}