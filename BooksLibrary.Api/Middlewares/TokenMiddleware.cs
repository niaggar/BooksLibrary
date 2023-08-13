using BooksLibrary.Core.Interfaces;

namespace BooksLibrary.Api.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate next;

        public TokenMiddleware(RequestDelegate nextRequest)
        {
            next = nextRequest;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenService = context.RequestServices.GetService(typeof(ITokenService)) as ITokenService;
                    var isValid = await tokenService.ValidateToken(token);
                    if (!isValid)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token");
                        return;
                    }
                }
            }

            await next(context);
        }
    }
}
