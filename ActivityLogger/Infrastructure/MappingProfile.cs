using ActivityLogger.Data.Models;
using ActivityLogger.Dtos;
using AutoMapper;

namespace ActivityLogger.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-ddTHH:mm:ssK")))
                .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.EndDate.ToString("yyyy-MM-ddTHH:mm:ssK")));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.Name));
        }
    }
}