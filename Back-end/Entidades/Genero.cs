using Back_end.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Back_end.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50)]
        [CapitalLetter]
        public string Nombre { get; set; }

    }
}
