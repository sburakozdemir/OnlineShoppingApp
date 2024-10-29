using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.DataProtection
{
    // DataProtection sınıfı, IDataProtection arayüzünü uyguluyor
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector; // IDataProtector arayüzüne ait bir örnek

        // Constructor: IDataProtectionProvider üzerinden bir IDataProtector oluşturuyor
        public DataProtection(IDataProtectionProvider provider)
        {
            // "security" anahtarı ile bir koruyucu oluştur
            _protector = provider.CreateProtector("security");
        }

        // Veriyi koruma metodu
        public string Protect(string text)
        {
            // Gelen metni koru ve geri döndür
            return _protector.Protect(text);
        }

        // Korumalı veriyi geri çözme metodu
        public string Unprotect(string protectedText)
        {
            // Korunan metni çöz ve geri döndür
            return _protector.Unprotect(protectedText);
        }
    }
}
