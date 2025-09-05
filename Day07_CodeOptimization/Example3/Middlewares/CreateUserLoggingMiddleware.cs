using System.Text;
using System.Text.Json;
using Code_Optimization_Example_3_solution.Models;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Code_Optimization_Example_3_solution.Middlewares
{
    public class CreateUserLoggingMiddleware:IMiddleware
    {
        private readonly ILogger<CreateUserLoggingMiddleware> logger;

        public CreateUserLoggingMiddleware(ILogger<CreateUserLoggingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var controllerActionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;

            if (actionName.Equals("CreateUser", StringComparison.OrdinalIgnoreCase) && controllerName.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();
               

                var userViewModel = JsonSerializer.Deserialize<UserViewModel>(context.Request.Body);

                logger.LogInformation($"User {userViewModel.FirstName} {userViewModel.LastName} is created.");
                context.Request.Body.Position = 0;
            }
            await next(context);
        }

    }
}
