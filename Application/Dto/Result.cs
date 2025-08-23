using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record Result<T>
    where T : notnull
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; } = true;

        public static Result<T> Success(T data, string message = "Success") =>
            new Result<T> { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Failure(string message) =>
            new Result<T> { IsSuccess = false, Data = default!, Message = message };

         

        public TResult Match<TResult>(Func<T?, TResult> onSuccess, Func<string, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Data) : onFailure(Message!);
        }

    }

}
