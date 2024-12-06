using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(100)]
    public string Product { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
}