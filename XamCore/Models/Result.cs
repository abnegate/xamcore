namespace XamCore.Models
{
    public class Result
    {
        public virtual bool WasSuccessful { get; set; }
        public string? Message { get; set; }
        public int? StatusCode { get; set; }

        public static Result Success(
            string? message = null,
            int? statusCode = null) => new() {
                WasSuccessful = true,
                Message = message,
                StatusCode = statusCode
            };

        public static Result Failure(
            string message,
            int statusCode = 400
        ) => new() {
            WasSuccessful = false,
            Message = message ?? "Unknown error",
            StatusCode = statusCode
        };
    }

    public sealed class Result<TModel> : Result
    {
        public TModel? Data { get; set; }

        public override bool WasSuccessful
        {
            get => base.WasSuccessful && Data is not null;
            set => base.WasSuccessful = value;
        }

        public static Result<TModel> Success(
            TModel data,
            string? message = null,
            int? statusCode = null) => new() {
                WasSuccessful = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };

        public new static Result<TModel?> Failure(
            string message,
            int statusCode = 500) => new() {
                WasSuccessful = false,
                Message = message ?? "Unknown error",
                StatusCode = statusCode
            };
    }

    public sealed class Result<T1, T2> : Result
    {
        public T1? Data1 { get; private set; }
        public T2? Data2 { get; private set; }

        public override bool WasSuccessful
        {
            get => base.WasSuccessful
                && Data1 is not null
                && Data2 is not null;
            set => base.WasSuccessful = value;
        }

        public static Result<T1, T2> Success(
            T1 data1,
            T2 data2,
            string? message = null,
            int? statusCode = null) => new() {
                WasSuccessful = true,
                Data1 = data1,
                Data2 = data2,
                Message = message,
                StatusCode = statusCode
            };

        public new static Result<T1?, T2?> Failure(
            string message,
            int statusCode = 500) => new() {
                WasSuccessful = false,
                Message = message ?? "Unknown error",
                StatusCode = statusCode
            };
    }
}
