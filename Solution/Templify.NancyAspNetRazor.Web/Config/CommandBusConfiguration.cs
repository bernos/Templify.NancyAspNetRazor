using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nancy.Bootstrapper;
using Templify.NancyAspNetRazor.Data.Commands;

namespace Templify.NancyAspNetRazor.Web.Config
{
    public class CommandBusConfiguration : IRegistrations
    {
        private readonly ICollection<TypeRegistration> _typeRegistrations;
 
        public CommandBusConfiguration()
        {
            _typeRegistrations = new Collection<TypeRegistration>
            {
                new TypeRegistration(typeof(ICommandHandler<LoginCommand, LoginCommandResult>), typeof(LoginCommandHandler)),
                new TypeRegistration(typeof(ICommandHandler<RegisterUserCommand, RegisterUserCommandResult>), typeof(RegisterUserCommandHandler))
            };
        }

        public IEnumerable<TypeRegistration> TypeRegistrations { get { return _typeRegistrations; } }
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations { get { return null; } }
        public IEnumerable<InstanceRegistration> InstanceRegistrations { get { return null; } }
    }
}