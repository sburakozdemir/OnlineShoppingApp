using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    public class OrderProductEntity:BaseEntity
    {
        public int OrderId { get; set; }
        

        public int ProductId { get; set; }
       

        public int Quantity { get; set; }

        public virtual OrderEntity Order { get; set; } // İlişkilendirilen sipariş
        public virtual ProductEntity Product { get; set; } // İlişkilendirilen ürün
    }
    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            // Id'yi yok say
            builder.Ignore(x => x.Id);

            // Birincil anahtar olarak OrderId ve ProductId'yi ayarla
            builder.HasKey(op => new { op.OrderId, op.ProductId });

            // İlişkiyi tanımla (isteğe bağlı)
            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            builder.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            base.Configure(builder);
        }
    }
}
