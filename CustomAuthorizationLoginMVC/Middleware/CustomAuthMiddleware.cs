using CustomAuthorizationLoginMVC.Database.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;

namespace CustomAuthorizationLoginMVC.Middleware
{
    public  class CustomAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (allowedList.Contains(context.Request.Path.Value))
                {
                    goto Result;
                }

                var token = context.Request.Cookies["Authorization"];
                if (token == null)
                {
                    context.Response.Redirect("/Login");
                    return;
                }
                if (token is not null)
                {
                    var tokenParts = token.Split(':');
                    if (tokenParts.Length != 2)
                    {
                        context.Response.Redirect("/Login");
                        return;
                    }

                    int userId = Convert.ToInt32(tokenParts[0]);
                    string sessionId = Convert.ToString(tokenParts[1]);

                    var db = context.RequestServices.GetRequiredService<AppDbContext>();
                    var user = db.TblLogin.FirstAsync(x => x.UserId == userId && x.SessionId == sessionId && x.SessionExpired > DateTime.Now);
                    if (user == null)
                    {
                        context.Response.Redirect("/Login");
                        return;
                    }

                }
            Result:
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Login");
                return;
            }


       
        }

        private string[] allowedList =  {
            "/",
            "/Login",
            "/Login/Index"
        };
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthMiddleware>();
        }
    }

}

