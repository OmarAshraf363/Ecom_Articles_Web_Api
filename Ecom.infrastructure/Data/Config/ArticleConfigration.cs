using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class ArticleConfigration : IEntityTypeConfiguration<Article>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(250);
        }
    }
}
