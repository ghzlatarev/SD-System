using Microsoft.AspNetCore.Http;
using SD.Services.Data.Exceptions;
using System.Threading.Tasks;

namespace SD.Web.Utilities.Middleware
{
    public class PageNotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public PageNotFoundMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/404");
                }
            }
            catch (EntityNotFoundException ex)
            {
                context.Response.Redirect("/404");
            }
        }
    }
}
