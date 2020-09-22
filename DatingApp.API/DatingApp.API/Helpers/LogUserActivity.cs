using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    // This helper method is used to update a property inside an object whenever they interact with the api
    public class LogUserActivity : IAsyncActionFilter
    {
        // ActionExecutingContext does something when the action is executed
        // ActionExecutionDelegate allows us to run code after action has been executed
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // waiting until action has been completed
            var resultContext = await next();
            
            // Get user id from jwt token
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Get instance of our repository by requesting the services
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            // Get users
            var user = await repo.GetUser(userId);

            // Update users last active date
            user.LastActive = DateTime.Now;

            // save changes
            await repo.SaveAll();

        }
    }
}
