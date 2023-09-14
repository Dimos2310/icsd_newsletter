using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;

public class MyActionFilter : ActionFilterAttribute, IActionFilter  
{  
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Get the Authorization header from the request
        string authorizationHeader = context.HttpContext.Request.Headers["exousiodotisi"];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            // Handle the case where there is no Authorization header
            context.Result = new UnauthorizedResult();
            return;
        }

        // You can perform your authentication logic here.
        // Example: Validate the token or credentials

        base.OnActionExecuting(context);
    }
}  