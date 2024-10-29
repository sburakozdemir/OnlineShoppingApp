using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Setting;
using System.Threading.Tasks;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService; // Ayar servisi referansı

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService; // Servisi atama
        }

        // Bakım modunu açma/kapama
        [HttpPatch]
        [Authorize(Roles = "Admin")] // Sadece Admin rolü ile erişilebilir
        public async Task<IActionResult> ToggleMaintenance()
        {
            await _settingService.ToggleMaintenance(); // Bakım modunu değiştir
            return Ok(new { Message = "Maintenance mode toggled." }); // Başarılı mesajı
        }

        // Bakım modu durumunu alma
        [HttpGet("state")]
        [Authorize(Roles = "Admin")] // Sadece Admin rolü ile erişilebilir
        public IActionResult GetMaintenanceState()
        {
            var isMaintenanceMode = _settingService.GetMaintenanceState(); // Bakım modunun durumu
            return Ok(new { MaintenanceMode = isMaintenanceMode }); // Durumu döndür
        }
    }
}
