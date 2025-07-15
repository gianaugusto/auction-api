namespace AutoAuction.API.Dtos
{
    public class ApiResponseWithData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }
}
