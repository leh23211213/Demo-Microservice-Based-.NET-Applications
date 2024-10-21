using System.ComponentModel.DataAnnotations;

namespace App.Frontend.Models;

public class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a Brand.")]
    public string? Name { get; set; }

    public string? ImageUrl { get; set; }
}
