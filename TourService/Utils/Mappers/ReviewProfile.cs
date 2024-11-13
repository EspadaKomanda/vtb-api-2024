using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Models.DTO;

namespace TourService.Utils.Mappers
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
                .ForMember(dest => dest.Benefits, opt => opt.Ignore())
                .ForMember(dest => dest.NegativeFeedbacksCount, opt => opt.Ignore())
                .ForMember(dest => dest.PositiveFeedbacksCount, opt => opt.Ignore());
            CreateProjection<Review, ReviewDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
                .ForMember(dest => dest.Benefits, opt => opt.Ignore())
                .ForMember(dest => dest.NegativeFeedbacksCount, opt => opt.Ignore())
                .ForMember(dest => dest.PositiveFeedbacksCount, opt => opt.Ignore());
            CreateMap<ReviewFeedback, FeedbackDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsPositive, opt => opt.MapFrom(src => src.IsPositive));
            CreateProjection<ReviewFeedback, FeedbackDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsPositive, opt => opt.MapFrom(src => src.IsPositive));
        }
    }
}