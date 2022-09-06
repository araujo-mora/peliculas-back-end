using System.ComponentModel.DataAnnotations;

namespace Back_end.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public string Trailer { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Poster { get; set; }
    }
}
