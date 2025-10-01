using EcommerceInventory.Application.Discounts;
using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Application.Services;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Domain.Entities.Orders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EcommerceInventory.Tests;
public class ConcurrentOrderTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly Mock<IBackgroundTaskQueue> _taskQueueMock;
    private readonly Mock<IDiscountStrategyFactory> _discountStrategyFactoryMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly OrderService _orderService;

    public ConcurrentOrderTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<OrderService>>();
        _paymentServiceMock = new Mock<IPaymentService>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _taskQueueMock = new Mock<IBackgroundTaskQueue>();
        _discountStrategyFactoryMock = new Mock<IDiscountStrategyFactory>();

        _productRepoMock = new Mock<IProductRepository>();
        _orderRepoMock = new Mock<IOrderRepository>();

        _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Orders).Returns(_orderRepoMock.Object);

        _orderService = new OrderService(
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _paymentServiceMock.Object,
            _eventPublisherMock.Object,
            _taskQueueMock.Object,
            _discountStrategyFactoryMock.Object
        );
    }

    [Fact]
    public async Task ProcessOrderAsync_Should_CompleteOrder_When_PaymentSucceeds()
    {
        // Arrange
        var product = new Product(price: 50m, name: "TestProduct", stock: 10);
        var order = new Order(Guid.NewGuid());
        order.AddItem(product.Id, 2, product.Price);
        order.SetDiscountStrategy(new NoDiscountStrategy());
        order.ApplyDiscount();

        _unitOfWorkMock.Setup(u => u.Orders.GetOrderByIdWithItemsAsync(order.Id))
            .ReturnsAsync(order);

        _unitOfWorkMock.Setup(u => u.Products.GetProductByIdAsync(product.Id))
            .ReturnsAsync(product);

        _paymentServiceMock.Setup(p => p.ProcessPaymentAsync(It.IsAny<decimal>()))
            .ReturnsAsync(true);

        _eventPublisherMock.Setup(e => e.PublishAsync(It.IsAny<OrderCompletedEvent>()))
            .Returns(Task.CompletedTask);

        // Act
        await _orderService.ProcessOrderAsync(order.Id);

        // Assert

        product.Stock.Should().Be(8); // 10 - 2
        _paymentServiceMock.Verify(p => p.ProcessPaymentAsync(order.FinalAmount), Times.Once);
        _eventPublisherMock.Verify(e => e.PublishAsync(It.IsAny<OrderCompletedEvent>()), Times.Once);
    }


}
