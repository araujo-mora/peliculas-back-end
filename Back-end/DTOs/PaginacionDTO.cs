namespace Back_end.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int offset = 10;
        private readonly int maxCount = 50;

        public int RecordsPorPagina { get { return offset; } set { offset = (value > maxCount) ? maxCount : value; } }
    }
}
