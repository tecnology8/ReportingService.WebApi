using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingServices.WebApi
{
    public class SetLanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public SetLanguageMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

        }
    }
}
