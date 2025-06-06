using AutoMapper;
using Products.Api.Models;
using Products.WebAPI.Models.Domain;

namespace Products.Api.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<ProductDto, Product>();
        }
    }
}
