using TodoDemo.Core.SharedKernel;

namespace TodoDemo.Core.Interfaces
{
    public interface IHandler<T> where T: BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}
