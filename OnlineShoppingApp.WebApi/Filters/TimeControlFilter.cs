using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc; 

namespace OnlineShoppingApp.WebApi.Filters
{
    // Zaman kontrolü yapan bir action filter
    public class TimeControlFilter : ActionFilterAttribute
    {
        // Başlangıç ve bitiş saatlerini tutan özellikler
        public string StartTime { get; set; } // Başlangıç saati
        public string EndTime { get; set; } // Bitiş saati

        // Eylem çağrılmadan önce çalıştırılacak olan metod
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Şu anki saat
            var now = DateTime.Now.TimeOfDay;

            // Filtre için sabit başlangıç ve bitiş saatlerini belirle
            StartTime = "23:00"; // Filtrenin aktif olacağı başlangıç saati
            EndTime = "23:59"; // Filtrenin aktif olacağı bitiş saati

            // Eğer şu anki saat belirtilen zaman aralığındaysa
            if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
            {
                // Eylemi devam ettir
                base.OnActionExecuting(context);
            }
            else
            {
                // Belirtilen zaman aralığında değilse 403 Forbidden durumu döndür
                context.Result = new ContentResult
                {
                    Content = "Bu saatler arasında bu endpoint'e istek atılamaz", // Hata mesajı
                    StatusCode = 403 // 403 Forbidden durumu
                };
            }
        }
    }
}
