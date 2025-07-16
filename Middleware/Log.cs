using Microsoft.AspNetCore.Http;
using System.Diagnostics;
namespace Core.Middleware;

public class LogMiddleware{
    private RequestDelegate next;

    public LogMiddleware(RequestDelegate next){
        this.next = next;
    }

    public async Task Invoke(HttpContext c){
        await c.Response.WriteAsync($"Log Middleware start\n");

        var sw = new Stopwatch();
        sw.Start();

        await next(c);

        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" Success: {c.Items["success"]}"
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");

        await c.Response.WriteAsync("Log Middleware end\n");
    }
}

public static class LogMiddlewareHelper{
    public static void UseLog(this IApplicationBuilder app){
        app.UseMiddleware<LogMiddleware>();
    }
}