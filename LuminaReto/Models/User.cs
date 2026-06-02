using System.ComponentModel.DataAnnotations;

namespace LuminaReto.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Username")]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
}
