namespace EcommerceInventory.Application.Mappings;
using AutoMapper;
using EcommerceInventory.Application.DTO.DsicountDTO;
using EcommerceInventory.Domain.Entities.Discounts;

public class DiscountRuleMappingProfile : Profile
{
    public DiscountRuleMappingProfile()
    {

        CreateMap<CreateDiscountRuleDto, DiscountRule>();

        CreateMap<UpdateDiscountRuleDto, DiscountRule>();

        CreateMap<DiscountRule, DiscountRuleDto>();
    }
}

