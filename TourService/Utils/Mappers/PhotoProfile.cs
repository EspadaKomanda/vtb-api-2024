using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Models.DTO;

namespace TourService.Utils.Mappers
{
    public class PhotoProfile : Profile
    {
        
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.FileLink));
                
            CreateProjection<Photo, PhotoDto>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.FileLink));
        }
    
    }
}