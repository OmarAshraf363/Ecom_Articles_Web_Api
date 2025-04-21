using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class UserConfigration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
           
            builder.Property(e=>e.DisplayName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PicImage).IsRequired().HasDefaultValue("noimage.png");
            builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
            builder.Property(e => e.UserName).IsRequired().HasMaxLength(50);

        }

    }
   

}
