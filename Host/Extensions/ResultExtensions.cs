using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Host.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller) where T : class
        {
            return result.IsSuccess ? controller.Ok(result) : controller.BadRequest(result);
        }
    }
}
