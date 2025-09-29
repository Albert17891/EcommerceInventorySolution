using EcommerceInventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceInventory.Api.Attributes;

public class CustomAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.Items["User"] as User;

        if (user is null)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Authentication required" });
        }
    }
}
