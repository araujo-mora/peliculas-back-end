using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Utilities
{
    public static class HttpContentExtensions
    {
        public async static Task InsertPaginationParamsInHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if(httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double quantity = await queryable.CountAsync();

            httpContext.Response.Headers.Add("TotalQuantity", quantity.ToString());
        }
    }
}
