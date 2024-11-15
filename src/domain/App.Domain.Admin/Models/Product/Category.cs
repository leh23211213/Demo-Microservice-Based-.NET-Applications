using System.ComponentModel.DataAnnotations;

namespace App.Domain.Admin.Models;

public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a Category.")]
    public string? Name { get; set; }
}
