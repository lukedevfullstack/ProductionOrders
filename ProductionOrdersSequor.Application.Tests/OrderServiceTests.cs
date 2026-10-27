using Moq;
using ProductionOrdersSequor.Application.DTOs;
using ProductionOrdersSequor.Application.Services;
using ProductionOrdersSequor.Domain.Entities;
using ProductionOrdersSequor.Domain.Interfaces;

namespace ProductionOrdersSequor.Application.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderService = new OrderService(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task SetProductionAsync_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var dto = new SetProductionDto
        {
            Email = "naocadastrado@sequor.com.br",
            Order = "111",
            ProductionDate = "2026-01-01",
            ProductionTime = "10:30:00",
            Quantity = 10,
            MaterialCode = "abc",
            CycleTime = 30
        };

        _orderRepositoryMock.Setup(r => r.GetUserByEmailAsync(dto.Email))
            .ReturnsAsync((User)null);

        // Act
        var result = await _orderService.SetProductionAsync(dto);

        // Assert
        Assert.Equal(201, result.Status);
        Assert.Equal("E", result.Type);
        Assert.Contains("Usuário não cadastrado", result.Description);
    }

    [Fact]
    public async Task SetProductionAsync_ShouldReturnSuccess_WhenAllValidationsPass()
    {
        // Arrange
        var dto = new SetProductionDto
        {
            Email = "teste@sequor.com.br",
            Order = "111",
            ProductionDate = "2026-06-01",
            ProductionTime = "10:30:00",
            Quantity = 50,
            MaterialCode = "aaa",
            CycleTime = 30.3M
        };

        var user = new User("teste@sequor.com.br", "Teste User", new DateTime(2026, 01, 01), new DateTime(2026, 12, 31));
        var product = new Product("prod1", "Produto Teste", "img", 30.0m);
        product.ProductMaterials.Add(new ProductMaterial("prod1", "aaa"));

        // Passar o produto associado à ordem aqui:
        var order = new Order("111", 100.0m, "prod1"); ;

        _orderRepositoryMock.Setup(r => r.GetUserByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _orderRepositoryMock.Setup(r => r.GetByCodeAsync(dto.Order))
            .ReturnsAsync(order);

        _orderRepositoryMock.Setup(r => r.AddProductionAsync(It.IsAny<Production>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.CommitAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _orderService.SetProductionAsync(dto);

        // Assert
        Assert.Equal(200, result.Status);
        Assert.Equal("S", result.Type);
        Assert.Contains("sucesso", result.Description);
    }
}