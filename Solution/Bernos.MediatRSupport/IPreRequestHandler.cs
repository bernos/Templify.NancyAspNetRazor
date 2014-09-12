namespace Bernos.MediatRSupport
{
    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}