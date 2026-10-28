using System;
using System.ComponentModel.DataAnnotations;

namespace Demo1.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a product name.")]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        [Display(Name = "Quantity in stock")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid non-negative price.")]
        [Display(Name = "Unit price")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
    }
}
