using Bernos.MediatRSupport;
using Bernos.MediatRSupport.FluentValidation;
using Bernos.MediatRSupport.log4net;

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