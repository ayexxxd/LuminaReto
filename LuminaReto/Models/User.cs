using System.ComponentModel.DataAnnotations;

namespace LuminaReto.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    public string? email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
