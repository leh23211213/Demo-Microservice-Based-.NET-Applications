using System.ComponentModel.DataAnnotations;

namespace App.Frontend.Models;

public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a Category.")]
    public string? Name { get; set; }
}
