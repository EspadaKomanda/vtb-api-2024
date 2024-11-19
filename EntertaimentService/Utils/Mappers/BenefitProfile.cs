using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.Benefits;
using TourService.Models.Benefits;

namespace TourService.Utils.Mappers
{
    public class BenefitProfile : Profile
    {
        public BenefitProfile()
        {
            CreateMap<Benefit, BenefitDto>()
                .ForMember(dest=>dest.BenefitId,opt=>opt.MapFrom(x=>x.Id))
                .ForMember(dest=>dest.BenefitName,opt=>opt.MapFrom(x=>x.Name));
            CreateProjection<Benefit, BenefitDto>()
                .ForMember(dest=>dest.BenefitId,opt=>opt.MapFrom(x=>x.Id))
                .ForMember(dest=>dest.BenefitName,opt=>opt.MapFrom(x=>x.Name));
        }
    }
}