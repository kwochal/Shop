using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class ExcludeRolesAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] excludedRoles;

    public ExcludeRolesAttribute(params string[] roles)
    {
        excludedRoles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAuthorized = context.HttpContext.User.Identity.IsAuthenticated;

        if (isAuthorized)
        {
           
            if (excludedRoles.Any(role => context.HttpContext.User.IsInRole(role)))
            {
                context.Result = new ForbidResult(); 
            }
        }
    }
}