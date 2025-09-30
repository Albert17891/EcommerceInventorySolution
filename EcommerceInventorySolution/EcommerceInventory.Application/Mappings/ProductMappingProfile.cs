using AutoMapper;
using EcommerceInventory.Application.DTO.ProductDTO;
using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.Mappings;
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductRequestDto, Product>()
            .ConstructUsing(dto => new Product(dto.Name, dto.Stock, dto.Price));

        CreateMap<UpdateProductRequestDto, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductResponseDto>();
    }
}
