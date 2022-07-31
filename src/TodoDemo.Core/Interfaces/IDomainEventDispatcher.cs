using TodoDemo.Core.SharedKernel;

namespace TodoDemo.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}
