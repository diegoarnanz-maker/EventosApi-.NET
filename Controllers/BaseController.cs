using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventosApi.Utils;

namespace EventosApi.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected string Username => User?.Identity?.Name ?? "Anonimo";

        protected ApiResponse<T> SuccessResponse<T>(T data)
        {
            return new ApiResponse<T>(data);
        }

        protected ApiResponse<T> FailedResponse<T>(string message)
        {
            return new ApiResponse<T>(message);
        }

        protected bool IsAdmin()
        {
            return User.IsInRole("ADMIN");
        }
    }
}
