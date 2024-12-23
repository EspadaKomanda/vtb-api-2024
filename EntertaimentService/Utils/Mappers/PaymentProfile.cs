using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.PaymentMethod;
using EntertaimentService.Models.PaymentVariant;
using TourService.Models.PaymentMethod;

namespace EntertaimentService.Utils.Mappers
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodDto>()
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PaymentVariants, opt => opt.MapFrom(src => src.PaymentVariants));

            CreateProjection<PaymentMethod, PaymentMethodDto>()
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PaymentVariants, opt => opt.MapFrom(src => src.PaymentVariants));

            CreateMap<PaymentVariant, PaymentVariantDto>()
                .ForMember(dest => dest.PaymentVariantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId))
                .ForMember(dest => dest.PaymentVariantName, opt => opt.MapFrom(src => src.Name));

            CreateProjection<PaymentVariant, PaymentVariantDto>()
                .ForMember(dest => dest.PaymentVariantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId))
                .ForMember(dest => dest.PaymentVariantName, opt => opt.MapFrom(src => src.Name));
        
        }
    }
}