using AutoMapper;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Mappings
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<CreateTaskRequest, TaskEntity>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DataVencimento))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<TaskEntity, TaskResponse>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
