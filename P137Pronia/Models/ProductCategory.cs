using System.ComponentModel.DataAnnotations;

namespace P137Pronia.Models;

public class ProductCategory:BaseEntity
{
    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
