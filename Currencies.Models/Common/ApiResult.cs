namespace Currencies.Models
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
        public T Result { get; set; }
    }
}
