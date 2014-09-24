using Bernos.MediatRSupport;
using Bernos.MediatRSupport.FluentValidation;
using Bernos.MediatRSupport.log4net;
using Templify.NancyAspNetRazor.Data.Commands;
using Templify.NancyAspNetRazor.Data.Commands.Decorators;

namespace Templify.NancyAspNetRazor.Data
{
    public class MediatorConfiguration : DefaultMediatorConfiguration
    {
        protected override void Configure()
        {
            AddRequestDecorator("logger", typeof(LoggingDecorator<,>));
            AddRequestDecorator("validator", typeof(ValidationDecorator<,>));
            //AddPreRequestHandler(typeof(AuthorisationDecorator<>));

            //AddRequestHandler(typeof(LoginCommandHandler));
        }
    }
}