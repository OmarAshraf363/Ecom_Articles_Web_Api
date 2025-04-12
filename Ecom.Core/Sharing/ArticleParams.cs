using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
   public class ArticleParams
    {

        public string? Search { get; set; } 
		public int? CategoryId { get; set; } 

        public int MaxPageSize { get; set; } = 10;
		private int _pageSize=3;

		public int pageSize 
		{
			get { return _pageSize; }
			set { _pageSize = value>MaxPageSize?MaxPageSize:value; }
		}
		public int PageNumber { get; set; } = 1;

    }
}
