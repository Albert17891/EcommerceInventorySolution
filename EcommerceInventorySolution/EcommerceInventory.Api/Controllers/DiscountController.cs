using EcommerceInventory.Api.Attributes;
using EcommerceInventory.Application.DTO.DsicountDTO;
using EcommerceInventory.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceInventory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [CustomAuthorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateDiscountRuleDto dto)
    {
        await _discountService.AddDiscountRuleAsync(dto);
        return Ok("Discount rule created");
    }

    [CustomAuthorize]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateDiscountRuleDto dto)
    {
        await _discountService.UpdateDiscountRuleAsync(dto);
        return Ok("Discount rule updated");
    }

    [CustomAuthorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var discountRule = await _discountService.GetByIdAsync(id);

        if (discountRule == null)
            return NotFound("Discount rule not found");

        return Ok(discountRule);
    }

    [CustomAuthorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var discountRules = await _discountService.GetAllAsync();

        return Ok(discountRules);
    }
}
