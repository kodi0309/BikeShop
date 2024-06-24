using AutoMapper;
using Srv_Sale.Models;
using Srv_Sale.DTOs;
using Transit;

namespace Srv_Sale.Map
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Sale, SaleDto>()
                .IncludeMembers(x => x.Item);

            CreateMap<Item, SaleDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Frame, opt => opt.MapFrom(src => src.AdditionalProperties.Frame))
                .ForMember(dest => dest.Handlebar, opt => opt.MapFrom(src => src.AdditionalProperties.Handlebar))
                .ForMember(dest => dest.Brakes, opt => opt.MapFrom(src => src.AdditionalProperties.Brakes))
                .ForMember(dest => dest.WheelsTires, opt => opt.MapFrom(src => src.AdditionalProperties.WheelsTires))
                .ForMember(dest => dest.Seat, opt => opt.MapFrom(src => src.AdditionalProperties.Seat))
                .ForMember(dest => dest.DerailleursDrive, opt => opt.MapFrom(src => src.AdditionalProperties.DerailleursDrive))
                .ForMember(dest => dest.AdditionalAccessories, opt => opt.MapFrom(src => src.AdditionalProperties.AdditionalAccessories));

            CreateMap<NewSaleDto, Sale>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));

            CreateMap<NewSaleDto, Item>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.AdditionalProperties, opt => opt.MapFrom(src => new ItemProperties
                {
                    Frame = src.Frame,
                    Handlebar = src.Handlebar,
                    Brakes = src.Brakes,
                    WheelsTires = src.WheelsTires,
                    Seat = src.Seat,
                    DerailleursDrive = src.DerailleursDrive,
                    AdditionalAccessories = src.AdditionalAccessories
                }));

            //---------------------------------------------------------
            CreateMap<UpdateSaleDto, Sale>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Status>(src.Status)))
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => new Item
            {
                Brand = src.Brand,
                Model = src.Model,
                Year = src.Year,
                ImageUrl = src.ImageUrl,
                AdditionalProperties = new ItemProperties
                {
                    Frame = src.Frame,
                    Handlebar = src.Handlebar,
                    Brakes = src.Brakes,
                    WheelsTires = src.WheelsTires,
                    Seat = src.Seat,
                    DerailleursDrive = src.DerailleursDrive,
                    AdditionalAccessories = src.AdditionalAccessories
                }
            }));

            CreateMap<SaleDto, SaleCreated>();
            CreateMap<Sale, SaleUpdated>()
                .IncludeMembers(a => a.Item);
            CreateMap<Item, SaleUpdated>();
        }
    }
}