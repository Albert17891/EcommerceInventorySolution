using EcommerceInventory.Api.Attributes;
using EcommerceInventory.Application.DTO.OrderDTO;
using EcommerceInventory.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceInventory.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("PlaceOrder/{userId:guid}")]
    [CustomAuthorize]
    public async Task<IActionResult> PlaceOrder(Guid userId, string? discountCard, List<OrderItemDto> items)
    {
        var order = await _orderService.CreateOrderAsync(userId, items,discountCard);

        return Ok(order);
    }
}
