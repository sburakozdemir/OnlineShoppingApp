using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork; // UnitOfWork arayüzü
        private readonly IRepository<SettingEntity> _settingRepository; // Ayar repository'si

        // Constructor - IUnitOfWork ve IRepository bağımlılıklarını alır
        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        // Bakım modunun durumunu al
        public bool GetMaintenanceState()
        {
            // Ayar repository'sinden bakım durumu bilgisini al
            var maintenanceState = _settingRepository.GetById(1).MaintenanceMode;
            return maintenanceState; // Bakım durumunu döndür
        }

        // Bakım modunu aç/kapat
        public async Task ToggleMaintenance()
        {
            // Ayar repository'sinden ayarları al
            var setting = _settingRepository.GetById(1);
            setting.MaintenanceMode = !setting.MaintenanceMode; // Mevcut durumu tersine çevir
            _settingRepository.Update(setting); // Güncellenen ayarları repository'ye uygula

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
            }
            catch (Exception)
            {
                throw new Exception("Bakım durumu güncellenirken bir hata ile karşılaşıldı."); // Hata durumunda özel bir hata fırlat
            }
        }
    }
}
