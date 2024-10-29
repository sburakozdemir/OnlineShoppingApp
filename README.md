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

## API Uç Noktaları

### Kullanıcı Uç Noktaları
- **Kullanıcı Girişi**
  - **Yöntem:** `POST`
  - **Uç Nokta:** `/api/auth/login`
  - **Açıklama:** Kullanıcının sisteme giriş yapması için.
  
- **Kullanıcı Kayıt**
  - **Yöntem:** `POST`
  - **Uç Nokta:** `/api/auth/register`
  - **Açıklama:** Yeni bir kullanıcı kaydı oluşturur.

### Ürün Uç Noktaları
- **Ürün Ekleme**
  - **Yöntem:** `POST`
  - **Uç Nokta:** `/api/product/add`
  - **Açıklama:** Yeni bir ürün ekler.

- **Ürün Güncelleme**
  - **Yöntem:** `PUT`
  - **Uç Nokta:** `/api/product/update/{id}`
  - **Açıklama:** Belirtilen ID'ye sahip ürünü günceller.

- **Ürün Silme**
  - **Yöntem:** `DELETE`
  - **Uç Nokta:** `/api/product/delete/{id}`
  - **Açıklama:** Belirtilen ID'ye sahip ürünü siler.

- **Tüm Ürünleri Listeleme**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/api/product/all`
  - **Açıklama:** Tüm ürünleri listeler.

- **Ürün Detayı**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/api/product/{id}`
  - **Açıklama:** Belirtilen ID'ye sahip ürünün detaylarını getirir.

### Sipariş Uç Noktaları
- **Sipariş Oluşturma**
  - **Yöntem:** `POST`
  - **Uç Nokta:** `/api/order/add`
  - **Açıklama:** Yeni bir sipariş oluşturur.

- **Sipariş İptali**
  - **Yöntem:** `PATCH`
  - **Uç Nokta:** `/api/order/cancel`
  - **Açıklama:** Belirtilen siparişi iptal eder.

- **Siparişleri Listeleme**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/api/order/all`
  - **Açıklama:** Kullanıcıya ait tüm siparişleri listeler.

- **Sipariş Detayı**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/api/order/{id}`
  - **Açıklama:** Belirtilen ID'ye sahip siparişin detaylarını getirir.

### Ayarlar Uç Noktaları
- **Bakım Modunu Etkinleştirme**
  - **Yöntem:** `PATCH`
  - **Uç Nokta:** `/api/settings`
  - **Açıklama:** Bakım modunu açar veya kapatır.

- **Bakım Modu Durumunu Öğrenme**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/api/settings/state`
  - **Açıklama:** Bakım modunun aktif olup olmadığını kontrol eder.

### Swagger Uç Noktası
- **Swagger UI**
  - **Yöntem:** `GET`
  - **Uç Nokta:** `/swagger`
  - **Açıklama:** API dökümantasyonu için kullanıcı dostu bir arayüz sunar.


## Bakım Modu
Admin kullanıcılar, sistem bakımında olduğunda kullanıcıların erişimini kontrol etmek için bakım modunu etkinleştirebilirler. Bakım modunun durumu aşağıdaki uç noktalar aracılığıyla yönetilebilir:
- **Bakım Modunu Etkinleştirme:** `PATCH /api/settings`
- **Bakım Modu Durumunu Öğrenme:** `GET /api/settings/state`

## Katkıda Bulunanlar
- [Burak Özdemir](https://github.com/sburakozdemir) - Proje geliştiricisi

## İletişim
Sorularınız veya önerileriniz için [Burak Özdemir](mailto:sburakozdemir00@gmail.com
) ile iletişime geçebilirsiniz.
