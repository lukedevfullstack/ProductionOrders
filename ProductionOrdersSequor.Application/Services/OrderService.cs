using ProductionOrdersSequor.Application.DTOs;
using ProductionOrdersSequor.Application.Interfaces;
using ProductionOrdersSequor.Domain.Entities;
using ProductionOrdersSequor.Domain.Interfaces;
using System.Globalization;

namespace ProductionOrdersSequor.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<object> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersWithDetailsAsync();

        return new
        {
            orders = orders.Select(o => new
            {
                order = o.OrderCode,
                quantity = o.Quantity,
                productCode = o.Product?.ProductCode,
                productDescription = o.Product?.ProductDescription,
                image = o.Product?.Image,
                cycleTime = o.Product?.CycleTime,
                materials = o.Product?.ProductMaterials.Select(pm => new
                {
                    materialCode = pm.MaterialCode,
                    materialDescription = pm.Material?.MaterialDescription
                })
            })
        };
    }

    public async Task<object> GetProductionsByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
            return new { productions = new List<object>() };

        var productions = await _orderRepository.GetProductionsByEmailAsync(email);

        var orders = await _orderRepository.GetAllOrdersWithDetailsAsync();

        var result = productions.Select(p => {
            var matchingMaterial = orders
                .Where(o => o.OrderCode == p.Order)
                .SelectMany(o => o.Product?.ProductMaterials ?? new List<ProductMaterial>())
                .FirstOrDefault(pm => pm.MaterialCode == p.MaterialCode);

            return new
            {
                order = p.Order,
                email = p.Email,
                date = p.Date.ToString("M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                quantity = p.Quantity,
                materialCode = p.MaterialCode,
                materialDescription = matchingMaterial?.Material?.MaterialDescription ?? p.MaterialCode, // Retorna a descrição ou o código como fallback
                cycleTime = p.CycleTime
            };
        });

        return new { productions = result };
    }

    public async Task<ProductionResponseDto> SetProductionAsync(SetProductionDto dto)
    {
        // 1. Validação de E-mail
        var user = await _orderRepository.GetUserByEmailAsync(dto.Email);
        if (user == null)
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Usuário não cadastrado!" };
        }

        // 2. Validação de Ordem
        var order = await _orderRepository.GetByCodeAsync(dto.Order);
        if (order == null)
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Ordem não encontrada!" };
        }

        // 3. Validação de Data de Apontamento
        if (!DateTime.TryParse($"{dto.ProductionDate} {dto.ProductionTime}", out var productionDate))
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Data/Hora inválida!" };
        }

        if (!user.IsActiveAt(productionDate))
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Data fora do período permitido!" };
        }

        // 4. Validação de Quantidade
        if (!order.IsValidQuantity(dto.Quantity))
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Quantidade inválida!" };
        }

        // 5. Validação de Material
        bool materialExists = order.Product?.ProductMaterials.Any(pm => pm.MaterialCode == dto.MaterialCode) ?? false;
        if (!materialExists)
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Material não pertence à ordem!" };
        }

        // 6. Validação de Tempo de Ciclo
        if (dto.CycleTime <= 0)
        {
            return new ProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento Tempo de ciclo deve ser maior que zero!" };
        }

        string message = "Apontamento realizado com sucesso!";
        if (order.Product != null && dto.CycleTime < order.Product.CycleTime)
        {
            message = "Apontamento realizado com sucesso, porém o tempo de ciclo informado é menor que o cadastrado.";
        }

        // Criar entidade de Domínio
        var production = new Production(
            dto.Email,
            dto.Order,
            productionDate,
            dto.Quantity,
            dto.MaterialCode,
            dto.CycleTime
        );

        await _orderRepository.AddProductionAsync(production);
        await _unitOfWork.CommitAsync();

        return new ProductionResponseDto { Status = 200, Type = "S", Description = message };
    }
}
