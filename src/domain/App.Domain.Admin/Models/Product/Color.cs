using System.ComponentModel.DataAnnotations;

namespace App.Domain.Admin.Models;

public class Color
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a Color.")]
    public string? Name { get; set; }
}
