using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile() { 
            CreateMap<UserModel, User>().ReverseMap();

            CreateMap<ProductModel, Product>().ReverseMap();

            CreateMap<BrandModel, Brand>().ReverseMap();

            CreateMap<AgeGroupProductModel, AgeGroupProduct>().ReverseMap();

            CreateMap<OriginModel, Origin>().ReverseMap();

            CreateMap<CategoryModel, Category>().ReverseMap();

            CreateMap<FeedBackModel, Feedback>().ReverseMap();
            CreateMap<BlogModel, Post>().ReverseMap();

            CreateMap<AddressModel, Address>().ReverseMap();

            CreateMap<RoleModel, Role>().ReverseMap();
        }
    }
}
