using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1806_LittleJoy.API.ViewModels.RequestModels;
using NET1806_LittleJoy.API.ViewModels.ResponeModels;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services;
using NET1806_LittleJoy.Service.Services.Interface;
using Newtonsoft.Json;

namespace NET1806_LittleJoy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetAllProductPaging")]
        public async Task<IActionResult> GetListProductPagingAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var result = await _productService.GetAllProductPagingAsync(paginationParameter);

                if (result != null)
                {
                    var metadata = new
                    {
                        result.TotalCount,
                        result.PageSize,
                        result.CurrentPage,
                        result.TotalPages,
                        result.HasNext,
                        result.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    return Ok(result);
                }
                else
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Product is empty"
                    });
                }
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                };
                return BadRequest(responseModel);
            }
        }


        [HttpGet("{GetProductDetailById}")]
        public async Task<IActionResult> GetProductByIdAsync(int GetProductDetailById)
        {
            try
            {
                var productDetailModel = await _productService.GetProductByIdAsync(GetProductDetailById);
                if (productDetailModel == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Product does not exist"
                    });
                }
                return Ok(productDetailModel);
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        //[Authorize(Roles = "STAFF,ADMIN")]
        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProductAsync([FromBody] ProductRequestModel productRequestModel)
        {
            try
            {
                ProductModel productModel = new ProductModel()
                {
                    ProductName = productRequestModel.ProductName,
                    Price = productRequestModel.Price,
                    Description = productRequestModel.Description,
                    Weight = productRequestModel.Weight,
                    Quantity = productRequestModel.Quantity,
                    Image = productRequestModel.Image,
                    AgeId = productRequestModel.AgeId,
                    OriginId = productRequestModel.OriginId,
                    BrandId = productRequestModel.BrandId,
                    CateId = productRequestModel.CateId,
                    UnsignProductName = productRequestModel.UnsignProductName,
                };

                var result = await _productService.AddNewProductAsync(productModel);

                if (result)
                {
                    return Ok(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Product added successfully"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Failed to add the Product"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        //[Authorize(Roles = "ADMIN")]
        [HttpDelete("{removeProductById}")]
        public async Task<IActionResult> RemoveProductByIdAsync(int removeProductById)
        {
            try
            {
                var result = await _productService.DeleteProductByIdAsync(removeProductById);
                if (result)
                {
                    return Ok(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        Message = "Remove product success"
                    });
                }
                else
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "The product does not exist"
                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }


        //[Authorize(Roles = "STAFF,ADMIN")]
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductRequestModel productRequestModel)
        {
            try
            {
                ProductModel productModel = new ProductModel()
                {
                    Id = productRequestModel.Id,
                    ProductName = productRequestModel.ProductName,
                    Price = productRequestModel.Price,
                    Description = productRequestModel.Description,
                    Weight = productRequestModel.Weight,
                    Quantity = productRequestModel.Quantity,
                    Image = productRequestModel.Image,
                    IsActive = productRequestModel.IsActive,
                    AgeId = productRequestModel.AgeId,
                    OriginId = productRequestModel.OriginId,
                    BrandId = productRequestModel.BrandId,
                    CateId = productRequestModel.CateId,
                    UnsignProductName = productRequestModel.UnsignProductName,
                };

                var result = await _productService.UpdateProductAsync(productModel);

                if (result == null)
                {
                    return NotFound(new ResponseModels()
                    {
                        HttpCode = StatusCodes.Status404NotFound,
                        Message = "Can not update this Product"
                    });
                }

                return Ok(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status200OK,
                    Message = "Update product success"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModels()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message.ToString()
                });
            }
        }

    }
}
