# OnlineShoppingApp

**OnlineShoppingApp**, kullanıcıların ürünleri keşfedebileceği, sipariş verebileceği ve yöneticilerin ürünleri yönetebileceği bir çevrimiçi alışveriş platformudur. Bu proje, modern yazılım geliştirme teknikleri ve mimarisi kullanılarak geliştirilmiştir.

## İçindekiler
- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [API Uç Noktaları](#api-uç-noktaları)
- [Bakım Modu](#bakım-modu)
- [Katkıda Bulunanlar](#katkıda-bulunanlar)

## Özellikler
- **Kullanıcı Kimlik Doğrulama:** JWT (JSON Web Token) ile güvenli kullanıcı oturum açma.
- **Ürün Yönetimi:** Ürün ekleme, güncelleme, silme ve listeleme işlemleri.
- **Sipariş Yönetimi:** Kullanıcıların sipariş oluşturma, iptal etme ve güncelleme imkanı.
- **Bakım Modu:** Yalnızca admin kullanıcıların erişebileceği bakım modunu etkinleştirme.
- **Swagger Dökümantasyonu:** API uç noktalarını kolayca keşfetmek için kullanıcı dostu arayüz.

## Teknolojiler
- **.NET 8:** Uygulama, ASP.NET Core kullanılarak geliştirilmiştir.
- **Entity Framework Core:** Veritabanı işlemleri için güçlü bir ORM.
- **SQL Server:** Veritabanı yönetimi için kullanılıyor.
- **JWT:** Güvenli kimlik doğrulama sağlamak için.
- **Swagger:** API uç noktalarını belgelemek için.

## Kurulum
Projenizi yerel ortamda çalıştırmak için aşağıdaki adımları izleyin:

1. **Depoyu Klonlayın:**
   ```bash
   git clone https://github.com/sburakozdemir/OnlineShoppingApp.git
   cd OnlineShoppingApp
   ```

2. **Gerekli Paketleri Yükleyin:**
   ```bash
   dotnet restore
   ```

3. **Veritabanı Migrations'larını Uygulayın:**
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı Başlatın:**
   ```bash
   dotnet run
   ```

## Kullanım
Uygulama çalıştığında, tarayıcınızdan [http://localhost:5000/swagger](http://localhost:5000/swagger) adresine giderek Swagger UI üzerinden API uç noktalarınızı keşfedebilirsiniz.

### Örnek API Uç Noktaları
- **Kullanıcı Girişi:** `POST /api/auth/login`
- **Ürün Ekleme:** `POST /api/product/add`
- **Ürünleri Listeleme:** `GET /api/product/all`
- **Sipariş Oluşturma:** `POST /api/order/add`

## Bakım Modu
Admin kullanıcılar, sistem bakımında olduğunda kullanıcıların erişimini kontrol etmek için bakım modunu etkinleştirebilirler. Bakım modunun durumu aşağıdaki uç noktalar aracılığıyla yönetilebilir:
- **Bakım Modunu Etkinleştirme:** `PATCH /api/settings`
- **Bakım Modu Durumunu Öğrenme:** `GET /api/settings/state`

## Katkıda Bulunanlar
- [Burak Özdemir](https://github.com/sburakozdemir) - Proje geliştiricisi

## İletişim
Sorularınız veya önerileriniz için [Burak Özdemir](mailto:sburakozdemir00@gmail.com
) ile iletişime geçebilirsiniz.
