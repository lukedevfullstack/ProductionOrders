namespace ProductionOrdersSequor.Domain.Entities;

public class Product
{
    public string ProductCode { get; private set; }
    public string ProductDescription { get; private set; }
    public string Image { get; private set; }
    public decimal CycleTime { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public ICollection<ProductMaterial> ProductMaterials { get; private set; } = new List<ProductMaterial>();

    public Product(string productCode, string productDescription, string image, decimal cycleTime)
    {
        ProductCode = productCode;
        ProductDescription = productDescription;
        Image = image;
        CycleTime = cycleTime;
    }
}
