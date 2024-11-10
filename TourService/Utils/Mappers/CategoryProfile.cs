using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Models.Category;

namespace TourService.Utils.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x=>x.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x=>x.Name));
            CreateProjection<Category,CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x=>x.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x=>x.Name));
        }
    }
}