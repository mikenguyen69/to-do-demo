using TodoDemo.Core.Entities;
using TodoDemo.Infrastructure.Data;
using System.Linq;


namespace TodoDemo.Api.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(AppDbContext context)
        {            
            if (!context.ToDoItems.Any())
            {
                // Add ToDoItems
                context.ToDoItems.AddRange(
                    new ToDoItem
                    {
                        Id = 1,
                        Title = "Test Item 1",
                        Description = "Item 1 Description"
                    },
                    new ToDoItem
                    {
                        Id = 2,
                        Title = "Test Item 2",
                        Description = "Item 2 Description"
                    }
                );


                context.SaveChanges();
            }
        }
    }
}
