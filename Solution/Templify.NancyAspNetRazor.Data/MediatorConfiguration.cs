using Bernos.MediatRSupport;
using Templify.NancyAspNetRazor.Data.Commands.Decorators;

namespace Templify.NancyAspNetRazor.Data
{
    public class MediatorConfiguration : DefaultMediatorConfiguration
    {
        protected override void Configure()
        {
            AddRequestDecorator("logger", typeof(LoggingDecorator<,>));
            AddPreRequestHandler(typeof(AuthorisationDecorator<>));
            AddPreRequestHandler(typeof(ValidationDecorator<>));
        }
    }
}