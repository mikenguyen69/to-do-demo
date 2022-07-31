using Ardalis.GuardClauses;
using TodoDemo.Core.Events;
using TodoDemo.Core.Interfaces;

namespace TodoDemo.Core.Services
{
    public class ToDoItemService : IHandler<ToDoItemCompletedEvent>
    {
        public void Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do nothing
        }
    }
}
