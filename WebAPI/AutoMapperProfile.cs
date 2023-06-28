using AutoMapper;
using BusinessObjectLayer.Models;
using DTOs.Request;
using DTOs.Response;

namespace WebAPI
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
            CreateMap<UpdateCustomerRequest, Customer>();
            CreateMap<FlowerBouquet, FlowerBouquetResponse>();
            CreateMap<CreateFlowerBouquetRequest, FlowerBouquet>();
            CreateMap<UpdateFlowerBouquetRequest, FlowerBouquet>();
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderDetail, OrderDetailsResponse>();
            CreateMap<Order, OrdResponse>();
        }
    }
}
