using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosApi.Utils
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string? message = null)
        {
            Data = data;
            Message = message;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }
}