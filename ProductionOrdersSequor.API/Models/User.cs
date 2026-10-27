using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime EndDate { get; set; }
}