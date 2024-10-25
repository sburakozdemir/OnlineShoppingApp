using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShoppingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    public class UserEntity:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // Benzersiz olacak
        public string PhoneNumber { get; set; }
        public string Password { get; set; } // Şifre korunmalı
        public Role Role { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } // Kullanıcının siparişleri
    }
    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(15);

            // Diğer konfigürasyonlar
            base.Configure(builder);
        }
    }
}
