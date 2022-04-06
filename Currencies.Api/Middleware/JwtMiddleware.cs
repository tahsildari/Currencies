using Currencies.Data.Context;
using Currencies.Models;
using Currencies.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currencies.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Cookies["refreshToken"];

            if (token != null)
                await attachAccountToContext(context, token, userService);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, string token, IUserService userService)
        {
            try
            {
                var user = userService.GetByToken(token);

                if (user?.RefreshTokens?.Any(t => t.IsActive) ?? false)
                    context.Items["Account"] = user;
                else
                    context.Items["Account"] = null;
            }
            catch (Exception x)
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
