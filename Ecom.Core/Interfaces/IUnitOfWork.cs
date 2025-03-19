using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
  public  interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepo { get;  }
        public IArticleCategoryRepo ArticleCategoryRepo { get; }
        public IArticleRepository ArticleRepository { get;   }
        public IImageRepo ImageRepo { get;  }
        public ILikeRepo LikeRepo { get; }
        public ICommentRepo CommentRepo { get; }
        public IArticleRow ArticleRowRepo { get; }
        public Task Commit();
    }   
}
