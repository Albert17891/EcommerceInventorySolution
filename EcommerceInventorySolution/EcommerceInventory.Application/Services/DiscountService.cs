using AutoMapper;
using EcommerceInventory.Application.DTO.DsicountDTO;
using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Domain.Entities.Discounts;

namespace EcommerceInventory.Application.Services;
public class DiscountService : IDiscountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DiscountService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task AddDiscountRuleAsync(CreateDiscountRuleDto createDiscountRule)
    {
        await _unitOfWork.DiscountRules.AddDiscountRuelAsync(_mapper.Map<CreateDiscountRuleDto, DiscountRule>(createDiscountRule));

        await _unitOfWork.CompleteAsync();
    }

    public async Task<IEnumerable<DiscountRuleDto>> GetAllAsync()
    {
        var discountRules = await _unitOfWork.DiscountRules.GetAllAsync();

        return _mapper.Map<IEnumerable<DiscountRule>, IEnumerable<DiscountRuleDto>>(discountRules);
    }

    public async Task<DiscountRuleDto?> GetByIdAsync(Guid id)
    {
        var discountRule = await _unitOfWork.DiscountRules.GetByIdAsync(id);

        return _mapper.Map<DiscountRule?, DiscountRuleDto?>(discountRule);
    }

    public async Task UpdateDiscountRuleAsync(UpdateDiscountRuleDto updateDiscountRule)
    {
        _unitOfWork.DiscountRules.UpdateDiscountRule(_mapper.Map<UpdateDiscountRuleDto, DiscountRule>(updateDiscountRule));

        await _unitOfWork.CompleteAsync();
    }
}
