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
    public class OrderEntity:BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public virtual UserEntity Customer { get; set; } // Siparişi veren müşteri

        public virtual ICollection<OrderProductEntity> OrderProducts { get; set; } // Siparişin ürünleri
    }
    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Müşteri ile ilişki
            builder.HasOne(x => x.Customer) // Siparişi veren müşteri
                .WithMany() // Eğer UserEntity içerisinde OrderEntity'yi referans ediyorsanız buraya <OrderEntity> ekleyebilirsiniz.
                .HasForeignKey(x => x.CustomerId);

            // Diğer konfigürasyonlar
            base.Configure(builder);
        }
    }
}

