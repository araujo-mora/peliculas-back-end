using Back_end.DTOs;

namespace Back_end.Utilities
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)
                .Take(paginacionDTO.RecordsPorPagina);
        }
    }
}
