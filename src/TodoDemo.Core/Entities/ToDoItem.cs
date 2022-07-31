using TodoDemo.Core.Events;
using TodoDemo.Core.SharedKernel;

namespace TodoDemo.Core.Entities
{
    public class ToDoItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public void MarkComplete()
        {
            IsDone = true;
            Events.Add(new ToDoItemCompletedEvent(this));
        }   
    }
}
