using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        //Relations
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();

    }
}
