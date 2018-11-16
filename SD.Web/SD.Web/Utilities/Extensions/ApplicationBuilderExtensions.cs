using Microsoft.AspNetCore.Builder;
using SD.Web.Utilities.Middleware;

namespace SD.Web.Utilities.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseNotFoundExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<PageNotFoundMiddleware>();
        }
    }
}
