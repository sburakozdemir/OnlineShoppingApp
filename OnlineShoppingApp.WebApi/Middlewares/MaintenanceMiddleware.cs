using OnlineShoppingApp.Business.Operations.Setting; 

namespace OnlineShoppingApp.WebApi.Middlewares
{
    // Bakım modu kontrolü yapan middleware sınıfı
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next; // Sonraki middleware delegesi

        // Middleware sınıfının yapıcı metodu
        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next; // Sonraki middleware delegesini ata
        }

        // Middleware'in çağrılacağı metod
        public async Task Invoke(HttpContext context)
        {
            // ISettingService'i al
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            // Bakım modunun aktif olup olmadığını kontrol et
            bool maintenanceMode = settingService.GetMaintenanceState();

            // Belirli endpoint'ler için middleware'in geçişine izin ver
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context); // Sonraki middleware'i çağır
                return; // İşlemi sonlandır
            }

            // Eğer bakım modu aktifse
            if (maintenanceMode)
            {
                // Kullanıcıya bakım modunda olduğunu bildiren mesaj gönder
                await context.Response.WriteAsync("Şu anda hizmet verememekteyiz.");
            }
            else
            {
                // Bakım modu aktif değilse, bir sonraki middleware'i çağır
                await _next(context);
            }
        }
    }
}
