namespace Domain.DTOs.Result
{
    public class Result
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = default!;
    }

    public class Result<T> : Result
    {
        public T ResponseObject { get; set; } = default!;
    }
}
