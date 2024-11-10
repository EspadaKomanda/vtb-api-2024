using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.DTO;

namespace EntertaimentService.Utils.Mappers
{
    public class PhotoProfile : Profile
    {
        
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.FileLink))
                .ForMember(dest => dest.EntertaimentId, opt => opt.MapFrom(src => src.EntertaimentId));
                
            CreateProjection<Photo, PhotoDto>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.FileLink))
                .ForMember(dest => dest.EntertaimentId, opt => opt.MapFrom(src => src.EntertaimentId));
        }
    
    }
}