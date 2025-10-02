using EcommerceInventory.Application.Common.Policies;
using EcommerceInventory.Application.DTO.OrderDTO;
using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Domain.Entities.Orders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EcommerceInventory.Application.Services;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderService> _logger;
    private readonly IPaymentService _paymentService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IDiscountStrategyFactory _discountStrategyFactory;

    public OrderService(IUnitOfWork unitOfWOrk,
                        ILogger<OrderService> logger,
                        IPaymentService paymentService,
                        IEventPublisher eventPublisher,
                        IBackgroundTaskQueue taskQueue,
                        IDiscountStrategyFactory discountStrategyFactory)
    {
        _unitOfWork = unitOfWOrk;
        _logger = logger;
        _paymentService = paymentService;
        _eventPublisher = eventPublisher;
        _taskQueue = taskQueue;
        _discountStrategyFactory = discountStrategyFactory;
    }
    public async Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId, List<OrderItemDto> items, string? discoundCartType)
    {
        var order = new Order(userId);

        foreach (var item in items)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(item.ProductId);

            if (product == null)
                return new CreateOrderResponseDto(Guid.Empty, false, $"Product {item.ProductId} not found");

            if (product.Stock < item.Quantity)
                return new CreateOrderResponseDto(Guid.Empty, false, $"Insufficient stock for {product.Name}");

            order.AddItem(product.Id, item.Quantity, product.Price);
        }

        var discountStrategy = await _discountStrategyFactory.CreateAsync(discoundCartType, order.TotalAmount);
        order.SetDiscountStrategy(discountStrategy);
        order.ApplyDiscount();

        await _unitOfWork.Orders.AddOrderAsync(order);
        await _unitOfWork.CompleteAsync();

        _taskQueue.QueueBackgroundWorkItem(async (sp, token) =>
        {
            var orderService = sp.GetRequiredService<IOrderService>();
            await orderService.ProcessOrderAsync(order.Id);
        });

        return new CreateOrderResponseDto(order.Id, true, "Order created successfully");
    }

    public async Task ProcessOrderAsync(Guid orderId)
    {
        try
        {
            var order = await _unitOfWork.Orders.GetOrderByIdWithItemsAsync(orderId);

            if (order == null)
            {
                _logger.LogError("Order {OrderId} not found", orderId);
                return;
            }

            order.MarkAsProcessing();
            await _unitOfWork.CompleteAsync();


            bool stockUpdateSuccess = await UpdateProductStockAsync(order);

            if (!stockUpdateSuccess)
            {
                order.MarkAsFailed();

                await _unitOfWork.CompleteAsync();

                _logger.LogError("Stock update failed for order {OrderId} ", orderId);

                return;
            }

            _logger.LogInformation("Processing payment for order {OrderId}", orderId);
            bool paymentSuccess = await _paymentService.ProcessPaymentAsync(order.FinalAmount);

            if (!paymentSuccess)
            {
                await ReleaseProductStockAsync(order);

                order.MarkAsFailed();
                await _unitOfWork.CompleteAsync();

                _logger.LogWarning("Payment failed for order {OrderId}", orderId);
                return;
            }

            order.MarkAsCompleted();
            await _unitOfWork.CompleteAsync();

            await PublishOrderCompletedEventAsync(order);

            _logger.LogInformation("Order {OrderId} completed successfully", orderId);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error processing order {OrderId}", orderId);

            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdWithItemsAsync(orderId);
                if (order != null)
                {
                    order.MarkAsFailed();
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception innerEx)
            {
                _logger.LogError(innerEx, "Failed to mark order {OrderId} as failed", orderId);
            }
        }
    }

    private async Task ReleaseProductStockAsync(Order order)
    {
        var retryPolicy = PollyPolicies.CreateConcurrencyRetryPolicy();

        await retryPolicy.ExecuteAsync(async () =>
        {
            foreach (var item in order.Items)
            {
                var product = await _unitOfWork.Products.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Restock(item.Quantity);

                    _unitOfWork.Products.UpdateProduct(product);
                }
            }
        });
    }

    private async Task PublishOrderCompletedEventAsync(Order order)
    {
        var orderCompletedEvent = new OrderCompletedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            CompletedAt = DateTime.UtcNow,
        };

        await _eventPublisher.PublishAsync(orderCompletedEvent);
    }

    private async Task<bool> UpdateProductStockAsync(Order order)
    {
        var retryPolicy = PollyPolicies.CreateConcurrencyRetryPolicy();

        try
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                foreach (var item in order.Items)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(item.ProductId);

                    if (product is null)
                        throw new InvalidOperationException($"Product {item.ProductId} not found");

                    if (!product.TryPurchase(item.Quantity))
                        throw new InvalidOperationException($"Insufficient stock for product {item.ProductId}");

                    _unitOfWork.Products.UpdateProduct(product);
                }

                await _unitOfWork.CompleteAsync();
            });

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stock for order {OrderId}", order.Id);
            return false;
        }
    }
}
