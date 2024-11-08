using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Models.PaymentMethod;
using TourService.Models.PaymentVariant;

namespace TourService.Utils.Mappers
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