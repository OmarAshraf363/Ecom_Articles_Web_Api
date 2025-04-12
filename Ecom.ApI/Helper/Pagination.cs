using Ecom.Core.DTO;

namespace Ecom.ApI.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageSize, int pageNumber, int totalCount, IEnumerable<T> data)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
