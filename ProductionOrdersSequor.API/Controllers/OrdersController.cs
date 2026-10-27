using Microsoft.AspNetCore.Mvc;
using ProductionOrdersSequor.Application.Interfaces;
using ApplicationDTOs = ProductionOrdersSequor.Application.DTOs;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrders() => Ok(await _orderService.GetOrdersAsync());

    [HttpGet("GetProduction")]
    public async Task<IActionResult> GetProduction([FromQuery] string email) => Ok(await _orderService.GetProductionsByEmailAsync(email));

    [HttpPost("SetProduction")]
    public async Task<IActionResult> SetProduction([FromBody] ApplicationDTOs.SetProductionDto dto)
    {
        var result = await _orderService.SetProductionAsync(dto);
        return StatusCode(200, result);
    }
}