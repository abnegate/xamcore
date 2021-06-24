using System;
namespace XamCore.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }

    public class Response<TModel> : Response
    {
        public TModel? Data { get; set; }
    }
}
