using Ecom.Core.Entites.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        //protected AppDbContext()
        //{
        //}
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<ArticleRow> ArticleRows { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ArticleCategory>().HasKey(ac => new { ac.ArticleId, ac.CategoryId });
            //builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
