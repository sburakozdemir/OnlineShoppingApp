using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace OnlineShoppingApp.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // İstekleri bir sonraki middleware veya işlemle devam ettirir
                await _next(context);
            }
            catch (Exception ex)
            {
                // Hata yakalandığında özel bir yanıt gönder
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Standart bir hata yanıtı oluşturma
            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = "Bir hata oluştu, lütfen tekrar deneyin." });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            // Hata mesajını döndür
            return context.Response.WriteAsync(result);
        }
    }
}
