
using EcommerceInventory.Api.Attributes;
using EcommerceInventory.Application.DTO.ProductDTO;
using EcommerceInventory.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceInventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> Create(CreateProductRequestDto dto)
    {
        var result = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [CustomAuthorize]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequestDto dto)
    {
        var result = await _productService.UpdateProductAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllProductsAsync();

        return Ok(result);
    }   
}

