using AutoMapper;
using TodoDemo.Api.DTO;
using TodoDemo.Core.Entities;

namespace TodoDemo.Api.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ToDoItem, ToDoItemDTO>();
                cfg.CreateMap<ToDoItemDTO, ToDoItem>();
            });

            return config.CreateMapper();
        }
    }
}
