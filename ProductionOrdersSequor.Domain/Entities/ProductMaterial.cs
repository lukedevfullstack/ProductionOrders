namespace ProductionOrdersSequor.Domain.Entities;

public class ProductMaterial
{
    public string ProductCode { get; private set; }
    public Product Product { get; private set; }

    public string MaterialCode { get; private set; }
    public Material Material { get; private set; }

    public ProductMaterial(string productCode, string materialCode)
    {
        ProductCode = productCode;
        MaterialCode = materialCode;
    }
}
