using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using EFConnect.Contracts;
using System;

namespace EFConnect.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userService = resultContext.HttpContext.RequestServices.GetService<IUserService>();
            var user = await userService.GetUserEntity(userId);

            user.LastActive = DateTime.Now;
            await userService.SaveAll();
        }
    }
}