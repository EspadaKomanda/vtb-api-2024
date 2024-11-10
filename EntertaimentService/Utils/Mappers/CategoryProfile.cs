using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.Category;

namespace EntertaimentService.Utils.Mappers
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