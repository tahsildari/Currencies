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
        private readonly IUserService _userService;

        public JwtMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["refreshToken"];

            if (token != null)
                await attachAccountToContext(context, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var user = _userService.GetByToken(token);

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
