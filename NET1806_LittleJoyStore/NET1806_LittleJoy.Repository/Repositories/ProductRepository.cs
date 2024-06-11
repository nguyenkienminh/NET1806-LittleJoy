using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class ProductRepository : IProductRepositoty
    {
        private readonly LittleJoyContext _context;

        public ProductRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Product>> GetAllProductPagingAsync(PaginationParameter paginationParameter)
        {

            var itemCount = await _context.Products.CountAsync();

            var item = await _context.Products.Include(p => p.Age)
                                              .Include(p => p.Brand)
                                              .Include(p => p.Cate)
                                              .Include(p => p.Origin)
                                              .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                              .Take(paginationParameter.PageSize)
                                              .AsNoTracking()
                                              .ToListAsync();

            var result = new Pagination<Product>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var item = await _context.Products
                .Include(p => p.Age)
                .Include(p => p.Brand)
                .Include(p => p.Cate)
                .Include(p => p.Origin)
                .SingleOrDefaultAsync(p => p.Id == productId);

            return item;
        }

        public async Task<Product> AddNewProductAsync(Product productInfo)
        {
            _context.Products.Add(productInfo);

            await _context.SaveChangesAsync();

            return productInfo;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            product.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Product> UpdateProductAsync(Product productModify, Product productPlace)
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

            await _context.SaveChangesAsync();
            return productPlace;
        }

        public async Task<Pagination<Product>> FilterProductPagingAsync(PaginationParameter paging, ProductFilterModel model)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(model.keyword))
            {
                products = products.Where(p => p.UnsignProductName.Contains(model.keyword));
            }

            if (model.cateId.HasValue)
            {
                products = products.Where(p => p.CateId == model.cateId);
            }

            if (model.brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == model.brandId);
            }

            if (model.ageId.HasValue)
            {
                products = products.Where(p => p.AgeId == model.ageId);
            }

            if (model.originId.HasValue)
            {
                products = products.Where(p => p.OriginId == model.originId);
            }


            if (model.sortOrder.HasValue)
            {
                switch (model.sortOrder)
                {
                    case 1:
                        products = products.OrderByDescending(p => p.Id);
                        break;

                    case 2:
                        products = products.OrderByDescending(p => p.Price);
                        break;

                    case 3:
                        products = products.OrderBy(p => p.Price);
                        break;
                }
            }

            var itemCount = await products.CountAsync();

            var item = await products.Skip((paging.PageIndex - 1) * paging.PageSize)
                                     .Take(paging.PageSize)
                                     .AsNoTracking()
                                     .ToListAsync();

            var result = new Pagination<Product>(item, itemCount, paging.PageIndex, paging.PageSize);

            return result;
        }

        public async Task<Pagination<Product>> GetAllProductOutOfStockPagingAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Products.CountAsync(p => p.Quantity < 10);

            var item = await _context.Products.Where(p => p.Quantity < 10)
                                              .Include(p => p.Age)
                                              .Include(p => p.Brand)
                                              .Include(p => p.Cate)
                                              .Include(p => p.Origin)
                                              .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                              .Take(paginationParameter.PageSize)
                                              .AsNoTracking()
                                              .ToListAsync();

            var result = new Pagination<Product>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
