using TodoDemo.Core.Entities;
using TodoDemo.Core.Interfaces;
using TodoDemo.Core.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        public override int SaveChanges()
        {
            int result = base.SaveChanges();

            // dispatch event if the save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }
    }
}
