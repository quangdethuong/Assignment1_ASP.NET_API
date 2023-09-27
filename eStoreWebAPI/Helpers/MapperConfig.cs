using AutoMapper;
using BusinessObject;
using DataAccess;
using eStoreWebAPI.DTO;
using eStoreWebAPI.DTO.Members;
using eStoreWebAPI.DTO.Order;
using eStoreWebAPI.DTO.Products;

namespace eStoreWebAPI.Helpers
{
    public class MapperConfig : Profile
    {

        public MapperConfig()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<ProductCreateDTO, Product>().ReverseMap();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
            CreateMap<MemberDTO, Member>().ReverseMap();
            CreateMap<LoginDTO, MemberDTO>().ReverseMap();
            CreateMap<SignUpDTO, Member>().ReverseMap();

            CreateMap<OrderDTO, Order>().ReverseMap();
            //CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();



        }
    }
}
