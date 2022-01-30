using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AdminPortalWeb.Models;

namespace AdminPortalWeb.Filters;

//This class is from day 6 McbaExampleWithLogin
public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var customerID = context.HttpContext.Session.GetString(nameof(Customer.CustomerID));
        if(!customerID.Equals("admin"))
            context.Result = new RedirectToActionResult("Index", "Login", null);
    }
}
