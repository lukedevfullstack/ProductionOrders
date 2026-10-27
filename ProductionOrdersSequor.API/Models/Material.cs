using System.ComponentModel.DataAnnotations;

public class Material
{
    [Key]
    public string MaterialCode { get; set; }
    public string MaterialDescription { get; set; }

    public ICollection<ProductMaterial> ProductMaterials { get; set; }
}