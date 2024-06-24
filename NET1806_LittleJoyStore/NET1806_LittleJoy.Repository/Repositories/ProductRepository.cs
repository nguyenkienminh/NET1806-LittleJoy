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

        public async Task<Product> UpdateProductAsync(Product productModify)
        {
            _context.Products.Update(productModify);

            await _context.SaveChangesAsync();
            return productModify;
        }

        public async Task<Pagination<Product>> FilterProductPagingAsync(PaginationParameter paging, ProductFilterModel model)
        {
            var products = _context.Products.AsQueryable();

            if(model.IsActive == true || model.IsActive.HasValue == false)
            {
                products = products.Where(p => p.IsActive == true);
            }
            else if(model.IsActive == false) 
            {
                products = products.Where(p => p.IsActive == false);
            }

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

        public async Task<Pagination<Product>> FilterStatusProductPagingAsync(PaginationParameter paging, ProductFilterStatusModel filterStatus)
        {
            var products = _context.Products.AsQueryable();

            if (filterStatus.status.HasValue)
            {
                switch (filterStatus.status)
                {
                    case 1:
                        products = products.Where(p => p.Quantity > 10);
                        break;

                    case 2:
                        products = products.Where(p => p.Quantity <= 10);
                        break;

                    case 3:
                        products = products.Where(p => p.Quantity == 0);
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
    }
}
