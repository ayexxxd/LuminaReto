using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class PerfilViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo no válido")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "El departamento es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    public string Departamento { get; set; }

    [Required(ErrorMessage = "El ID es obligatorio")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Solo números")]
    public string IdEmpleado { get; set; }

    // opcional
    [ValidateNever]
    public string FotoUrl { get; set; }
}