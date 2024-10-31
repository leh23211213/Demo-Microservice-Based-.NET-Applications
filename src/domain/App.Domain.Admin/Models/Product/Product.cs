using System.ComponentModel.DataAnnotations;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Models
{
    public class Product
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Name must not contain special characters.")]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }
        public string? Description { get; set; }
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public virtual Size? Size { get; set; }
        public virtual Color? Color { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Brand? Brand { get; set; }
        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}