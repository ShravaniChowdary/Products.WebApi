using System.ComponentModel.DataAnnotations;

namespace Products.Api.Models
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = default!;
    }
}
