using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- من Entity إلى DTO ---

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.Name)));

            CreateMap<Category, CategoryDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Tag, TagDto>();

            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.OrderItems.Count));

            CreateMap<Order, OrderDetailsDto>();
            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Payment, PaymentDto>();
            CreateMap<Shipping, ShippingDto>();

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<PromoCode, PromoCodeDto>();
            CreateMap<InventoryLog, InventoryLogDto>();

            CreateMap<Wishlist, WishlistDto>();
            CreateMap<WishlistItem, WishlistItemDto>();


            // يستخدم عند تسجيل مستخدم جديد
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // يستخدم عند إضافة تقييم جديد
            CreateMap<AddReviewDto, Review>();
        }
    }
}