using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    [Column("Order")]
    public string OrderCode { get; set; }
    public decimal Quantity { get; set; }
    public string ProductCode { get; set; }

    [ForeignKey("ProductCode")]
    public Product Product { get; set; }
}