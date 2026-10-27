using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public string ProductCode { get; set; }
    public string ProductDescription { get; set; }
    public string Image { get; set; }
    public decimal CycleTime { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<ProductMaterial> ProductMaterials { get; set; }
}