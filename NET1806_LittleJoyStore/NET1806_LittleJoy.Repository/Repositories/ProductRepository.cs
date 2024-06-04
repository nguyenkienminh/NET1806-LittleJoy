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
            _context.Add(productInfo);

            await _context.SaveChangesAsync();

            return productInfo;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            product.IsActive = false;

            _context.SaveChanges();

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
    }
}
