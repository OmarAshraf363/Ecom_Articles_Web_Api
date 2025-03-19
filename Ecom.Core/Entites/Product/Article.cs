using System;
using System.Collections.Generic;
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
        //Relations
        public virtual List<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
        public virtual List<ArticleRow> ArticleRows { get; set; } = new List<ArticleRow>();

    }
}
