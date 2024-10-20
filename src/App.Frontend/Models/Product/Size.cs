using System.ComponentModel.DataAnnotations;

namespace App.Frontend.Models;

public class Size
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a Size.")]
    public string? RAM { get; set; }
}
