using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Mapper;
using NET1806_LittleJoy.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositoty _productRepositoty;
        private readonly IMapper _mapper;

        public ProductService(IProductRepositoty productRepositoty, IMapper mapper)
        {
            _productRepositoty = productRepositoty;
            _mapper = mapper;
        }

        public async Task<Pagination<ProductModel>> GetAllProductPagingAsync(PaginationParameter paginationParameter)
        {
            var listProduct = await _productRepositoty.GetAllProductPagingAsync(paginationParameter);
            if (!listProduct.Any())
            {
                return null;
            }
            var listProductModels = _mapper.Map<List<ProductModel>>(listProduct);

            return new Pagination<ProductModel>(listProductModels,
                listProduct.TotalCount,
                listProduct.CurrentPage,
                listProduct.PageSize);

        }
    }
}
