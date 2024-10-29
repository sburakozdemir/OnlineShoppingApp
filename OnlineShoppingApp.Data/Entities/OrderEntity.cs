using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    // Sipariş varlığı
    public class OrderEntity : BaseEntity
    {
        // Sipariş tarihi
        public DateTime OrderDate { get; set; }

        // Toplam tutar
        public decimal TotalAmount { get; set; }

        // Siparişi veren müşterinin ID'si
        public int CustomerId { get; set; }

        // Siparişi veren müşteri
        public virtual UserEntity Customer { get; set; }

        // Siparişin içindeki ürünler
        public virtual ICollection<OrderProductEntity> OrderProducts { get; set; }
    }

    // Sipariş konfigürasyonu
    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            // Sipariş tarihinin zorunlu olduğunu belirt
            builder.Property(x => x.OrderDate)
                .IsRequired();

            // Toplam tutarın zorunlu olduğunu ve decimal olarak saklanacağını belirt
            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Müşteri ile ilişkiyi tanımla
            builder.HasOne(x => x.Customer) // Siparişi veren müşteri
                .WithMany() // Bir müşterinin birden fazla siparişi olabilir
                .HasForeignKey(x => x.CustomerId); // Müşteri ID'sini yabancı anahtar olarak ayarla

            // Diğer konfigürasyonları uygula
            base.Configure(builder);
        }
    }
}
