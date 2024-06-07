using AutoMapper;
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
            CreateMap<BlogModel, Post>().ReverseMap();

            CreateMap<AddressModel, Address>().ReverseMap();
        }
    }
}
