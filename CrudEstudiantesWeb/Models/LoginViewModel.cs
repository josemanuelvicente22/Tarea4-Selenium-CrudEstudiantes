using System.ComponentModel.DataAnnotations;

namespace CrudEstudiantesWeb.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;

        public string? MensajeError { get; set; }
    }
}
