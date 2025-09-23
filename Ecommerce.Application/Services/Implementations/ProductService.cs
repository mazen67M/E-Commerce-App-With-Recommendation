﻿using AutoMapper;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; // 1. حقن IMapper

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // Done
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products =  await _productRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Done
        public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync(int count)
        {
            var products = await _productRepository.GetFeaturedProductsAsync(count);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Done
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        // Done
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Done
        public async Task<IEnumerable<ProductDto>> GetTopSellingProductsAsync(int count)
        {
            var products = await _productRepository.GetTopSellingAsync(count);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Done
        public async Task<IEnumerable<ProductDto>> SearchProductAsync(string searchTerm, int? categoryId = null)
        {
            var products =  await _productRepository.SearchAsync(searchTerm, categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }



        //مش هنحتاجها لاننا استخدمنا AutoMapper
        //============================================
        //private ProductDto MapToProductDto(Product product)
        //{
        //    return new ProductDto
        //    {
        //        ProductID = product.ProductID,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        ImageURL = product.ImageURL,
        //        StockQuantity = product.StockQuantity,
        //        IsAvailable = product.IsAvailable,
        //        CategoryID = product.CategoryID,
        //        CategoryName = product.Category.Name,
        //        BrandID = product.BrandID,
        //        BrandName = product.Brand?.Name,
        //        AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
        //        ReviewCount = product.Reviews.Count
        //    };
    }
}

