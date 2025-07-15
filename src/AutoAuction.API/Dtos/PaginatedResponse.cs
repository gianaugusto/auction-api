using System.Collections.Generic;

namespace AutoAuction.API.Dtos
{
    public class PaginatedResponse<T> : ApiResponseWithData<IEnumerable<T>>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}