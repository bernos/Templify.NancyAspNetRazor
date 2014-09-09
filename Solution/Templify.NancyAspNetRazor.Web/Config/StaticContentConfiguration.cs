using System;
using Nancy.Conventions;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class StaticContentConfiguration : IConvention
    {
        public void Initialise(NancyConventions conventions)
        {
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", "Scripts"));
        }

        public Tuple<bool, string> Validate(NancyConventions conventions)
        {
            return Tuple.Create(true, string.Empty);
        }
    }
}