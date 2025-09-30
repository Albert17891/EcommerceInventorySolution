using AutoMapper;
using EcommerceInventory.Application.Common.Policies;
using EcommerceInventory.Application.DTO.ProductDTO;
using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Domain.Entities;
using Polly;
using System.Data;

namespace EcommerceInventory.Application.Services;
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAsyncPolicy _retryPolicy;

    public ProductService(IUnitOfWork unitOfWOrk, IMapper mapper)
    {
        _unitOfWork = unitOfWOrk;
        _mapper = mapper;
        _retryPolicy = PollyPolicies.CreateConcurrencyRetryPolicy();
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto createProductRequestDto)
    {
        var product = _mapper.Map<CreateProductRequestDto, Product>(createProductRequestDto);

        await _unitOfWork.Products.AddProductAsync(product);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<Product, ProductResponseDto>(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllProductsAsync();

        return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(Guid productId)
    {
        var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
        if (product == null) return null;

        return _mapper.Map<Product, ProductResponseDto>(product);
    }
   
    public async Task<ProductResponseDto?> UpdateProductAsync(Guid productId, UpdateProductRequestDto updateProductRequestDto)
    {
        var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
        if (product == null) return null;

        _mapper.Map(updateProductRequestDto, product);     

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<Product, ProductResponseDto>(product);
    }
}
