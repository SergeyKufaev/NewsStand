using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Producer, ProducerViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Purchase, PurchaseViewModel>().ReverseMap();
            CreateMap<PurchaseProduct, PurchaseProductViewModel>().ReverseMap();
        }
    }
}
