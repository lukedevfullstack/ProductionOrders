using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Production
{
    [Key]
    public long ID { get; set; }
    public string Email { get; set; }

    [Column("Order")]
    public string Order { get; set; }

    [Column("Date")]
    public DateTime Date { get; set; }
    public decimal Quantity { get; set; }
    public string MaterialCode { get; set; }
    public decimal CycleTime { get; set; }
}