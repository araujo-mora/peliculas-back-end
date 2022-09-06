using Back_end.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Back_end.DTOs
{
    public class CrearGeneroDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [CapitalLetter]
        public string? Nombre { get; set; }
    }
}
