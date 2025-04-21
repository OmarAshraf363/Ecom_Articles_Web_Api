using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
    public class Article:BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BaseImageUrl { get; set; } = string.Empty;
        
        public int Views { get; set; } = 0;
        public string AppUserId { get; set; } = string.Empty;
        
        [ForeignKey(nameof(AppUserId))]
        public AppUser User { get; set; } = new AppUser();
        //Relations
        public virtual List<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
        public virtual List<ArticleRow> ArticleRows { get; set; } = new List<ArticleRow>();
        public virtual List<Like> Likes { get; set; } = new List<Like>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
