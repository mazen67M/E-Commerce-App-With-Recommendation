using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.DTOs.Inventory;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.DTOs.Payment;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.DTOs.Promotion;
using Ecommerce.Application.DTOs.Review;
using Ecommerce.Application.DTOs.Shipping;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.DTOs.Wishlist;
using Ecommerce.Application.ViewModels; // For RegisterViewModel
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Ecommerce.Core.Entities;
using System.Linq;

namespace Ecommerce.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Product & Catalog Mappings ---
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Reviews != null && src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews != null ? src.Reviews.Count : 0))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Select(pt => pt.Tag.Name) : new List<string>()));

            CreateMap<Category, CategoryDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Tag, TagDto>();

            // --- Cart Mappings ---
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();

            // --- Order Mappings ---
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.OrderItems != null ? src.OrderItems.Count : 0));
            CreateMap<Order, OrderDetailsDto>();
            CreateMap<OrderItem, OrderItemDto>();

            // --- Payment & Shipping Mappings ---
            CreateMap<Payment, PaymentDto>();
            CreateMap<Shipping, ShippingDto>();

            // --- User & Auth Mappings ---
            CreateMap<ApplicationUser, UserDto>();
            // ➕ ADDED: Mapping for profile updates. .ReverseMap() creates the mapping from User -> UpdateUserDto as well.
            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();

            // --- Review Mappings ---
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    (src.User != null ? src.User.FirstName : "") + " " + (src.User != null ? src.User.LastName : "")));
            CreateMap<AddReviewDto, Review>();

            // --- Inventory Mappings ---
            CreateMap<InventoryLog, InventoryLogDto>();

            // --- Wishlist Mappings ---
            CreateMap<Wishlist, WishlistDto>();
            CreateMap<WishlistItem, WishlistItemDto>();

            // --- Promotion Mappings ---
            // ➕ ADDED: Missing mapping for PromoCode
            CreateMap<PromoCode, PromoCodeDto>();

            // --- ViewModel/DTO to Entity Mappings (For Inputs) ---
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}