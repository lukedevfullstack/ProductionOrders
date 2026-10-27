namespace ProductionOrdersSequor.Domain.Entities;
public class Material
{
    public string MaterialCode { get; private set; }
    public string MaterialDescription { get; private set; }

    public ICollection<ProductMaterial> ProductMaterials { get; private set; } = new List<ProductMaterial>();

    public Material(string materialCode, string materialDescription)
    {
        MaterialCode = materialCode;
        MaterialDescription = materialDescription;
    }
}
