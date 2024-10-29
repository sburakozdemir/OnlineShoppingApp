using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.DataProtection
{
    // IDataProtection arayüzü, veri koruma işlemleri için gerekli metotları tanımlar
    public interface IDataProtection
    {
        // Metni koruma metodu
        string Protect(string text);

        // Korunan metni geri çözme metodu
        string Unprotect(string protectedText);
    }
}
