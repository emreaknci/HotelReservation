using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.Interceptors;
public class DelayInterceptor
{
    private readonly RequestDelegate _next;

    public DelayInterceptor(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await Task.Delay(250);
        await _next.Invoke(context);
    }
}
