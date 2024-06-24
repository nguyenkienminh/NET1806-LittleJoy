using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Mapper;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositoty _productRepository;
        private readonly IMapper _mapper;
        private readonly IFeedBackRepository _feedBack;

        public ProductService(IProductRepositoty productRepository, IMapper mapper, IFeedBackRepository feedBack)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _feedBack = feedBack;
        }

        public async Task<Pagination<ProductModel>> GetAllProductPagingAsync(PaginationParameter paginationParameter)
        {
            var listProduct = await _productRepository.GetAllProductPagingAsync(paginationParameter);
            if (!listProduct.Any())
            {
                return null;
            }


            var listProductModels = listProduct.Select(p => new ProductModel
            {
                Id = p.Id,
                CateId = p.CateId,
                ProductName = p.ProductName,
                Price = p.Price,
                Description = p.Description,
                Weight = p.Weight,
                IsActive = p.IsActive,
                Quantity = p.Quantity,
                Image = p.Image,
                AgeId = p.AgeId,
                OriginId = p.OriginId,
                BrandId = p.BrandId,
                UnsignProductName = p.UnsignProductName, 
                RatingAver = _feedBack.AverageRatingAsync(p.Id).Result
            }).ToList();


            return new Pagination<ProductModel>(listProductModels,
                listProduct.TotalCount,
                listProduct.CurrentPage,
                listProduct.PageSize);
        }

        public async Task<ProductModel?> GetProductByIdAsync(int productId)
        {
            var productDetail = await _productRepository.GetProductByIdAsync(productId);

            if (productDetail == null)
            {
                return null;
            }

            var productModelInfo = _mapper.Map<ProductModel>(productDetail);
            productModelInfo.RatingAver = await _feedBack.AverageRatingAsync(productModelInfo.Id);
            return productModelInfo;
           
        }

        public async Task<bool> AddNewProductAsync(ProductModel productModel)
        {
            try
            {
                productModel.IsActive = true;

                var productInfo = _mapper.Map<Product>(productModel);

                productInfo.UnsignProductName = StringUtils.ConvertToUnSign(productInfo.ProductName);

                await _productRepository.AddNewProductAsync(productInfo);
                return true; 
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Fail to add product {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> DeleteProductByIdAsync(int productId)
        {

            var removeProduct = await _productRepository.GetProductByIdAsync(productId);

            if (removeProduct == null)
            {
                return false;
            }

            return await _productRepository.DeleteProductAsync(removeProduct);
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel productModel)
        {
            productModel.UnsignProductName = StringUtils.ConvertToUnSign(productModel.ProductName);

            var productModify = _mapper.Map<Product>(productModel);

            var productPlace = await _productRepository.GetProductByIdAsync(productModel.Id);

            if (productPlace == null)
            {
                return null;
            }
            else
            {
                productPlace.ProductName = productModify.ProductName;
                productPlace.Price = productModify.Price;
                productPlace.Description = productModify.Description;
                productPlace.Weight = productModify.Weight;
                productPlace.Quantity = productModify.Quantity;
                productPlace.Image = productModify.Image;
                productPlace.IsActive = productModify.IsActive;
                productPlace.AgeId = productModify.AgeId;
                productPlace.OriginId = productModify.OriginId;
                productPlace.BrandId = productModify.BrandId;
                productPlace.CateId = productModify.CateId;
                productPlace.UnsignProductName = productModify.UnsignProductName;
            }


            var updateProduct = await _productRepository.UpdateProductAsync(productPlace);

            if (updateProduct != null)
            {
                return _mapper.Map<ProductModel>(updateProduct);
            }
            return null;
        }

        public async Task<Pagination<ProductModel>> FilterProductPagingAsync(PaginationParameter paging, ProductFilterModel model)
        {

            if (model.sortOrder < 1 || model.sortOrder > 4)
            {
                throw new Exception("Thông tin không hợp lệ");
            }

            var listProduct =  await _productRepository.FilterProductPagingAsync(paging,model);

            if (!listProduct.Any())
            {
                return null;
            }

            var listProductModels = listProduct.Select(p => new ProductModel
            {
                Id = p.Id,
                CateId = p.CateId,
                ProductName = p.ProductName,
                Price = p.Price,
                Description = p.Description,
                Weight = p.Weight,
                IsActive = p.IsActive,
                Quantity = p.Quantity,
                Image = p.Image,
                AgeId = p.AgeId,
                OriginId = p.OriginId,
                BrandId = p.BrandId,
                UnsignProductName = p.UnsignProductName,
                RatingAver = _feedBack.AverageRatingAsync(p.Id).Result
            }).ToList();


            return new Pagination<ProductModel>(listProductModels,
                listProduct.TotalCount,
                listProduct.CurrentPage,
                listProduct.PageSize);
        }

        public async Task<Pagination<ProductModel>> FilterStatusProductPagingAsync(PaginationParameter paging, ProductFilterStatusModel filterStatus)
        {

            if(filterStatus.status < 1 || filterStatus.status > 4) 
            {
                throw new Exception("Thông tin không hợp lệ");
            }

            var listProduct = await _productRepository.FilterStatusProductPagingAsync(paging, filterStatus);

            if (!listProduct.Any())
            {
                return null;
            }

            var listProductModels = listProduct.Select(p => new ProductModel
            {
                Id = p.Id,
                CateId = p.CateId,
                ProductName = p.ProductName,
                Price = p.Price,
                Description = p.Description,
                Weight = p.Weight,
                IsActive = p.IsActive,
                Quantity = p.Quantity,
                Image = p.Image,
                AgeId = p.AgeId,
                OriginId = p.OriginId,
                BrandId = p.BrandId,
                UnsignProductName = p.UnsignProductName,
                RatingAver = _feedBack.AverageRatingAsync(p.Id).Result
            }).ToList();


            return new Pagination<ProductModel>(listProductModels,
                listProduct.TotalCount,
                listProduct.CurrentPage,
                listProduct.PageSize);
        }
    }
}
