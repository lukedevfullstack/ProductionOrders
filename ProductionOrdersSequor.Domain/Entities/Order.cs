namespace ProductionOrdersSequor.Domain.Entities;

public class Order
{
    public string OrderCode { get; private set; }
    public decimal Quantity { get; private set; }
    public string ProductCode { get; private set; }

    public Product Product { get; private set; }

    public Order(string orderCode, decimal quantity, string productCode)
    {
        OrderCode = orderCode;
        Quantity = quantity;
        ProductCode = productCode;
    }

    public bool IsValidQuantity(decimal quantityToProduce)
    {
        return quantityToProduce > 0 && quantityToProduce <= Quantity;
    }
}