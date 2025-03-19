using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
    public class Image:BaseEntity<int>
    {
        public string Url { get; set; }
        //relations
        public int? ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        public virtual Article Article { get; set; }

       
    }
}
