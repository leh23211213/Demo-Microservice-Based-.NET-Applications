using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using App.Services.ProductAPI.Extensions;
namespace App.Services.OrderAPI.Models;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
