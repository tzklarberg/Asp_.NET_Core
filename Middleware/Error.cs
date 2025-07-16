using System.Net;
using System.Net.Mail;
namespace Core.Middleware;

public class ErrorMiddleware
{
    private RequestDelegate next;

    public ErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        c.Items["success"] = false;
        bool success = false;
        try
        {
            await next(c);
            c.Items["success"] = true;
        }
        catch (ApplicationException ex)
        {
            c.Response.StatusCode = 400;
            await c.Response.WriteAsync(ex.Message);
        }
        catch (Exception e)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 465,
                Credentials = new NetworkCredential("Yaffi804@gmail.com", "Yaffi.Alt8"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("Yaffi804@gmail.com"),
                Subject = "Error",
                Body = e.Message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add("Yaffi804@gmail.com");
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static WebApplication UseError(this WebApplication app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }
}