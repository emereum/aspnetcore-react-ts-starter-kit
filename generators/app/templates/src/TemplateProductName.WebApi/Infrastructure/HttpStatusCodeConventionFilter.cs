using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateProductName.Common.Errors;

namespace TemplateProductName.WebApi.Infrastructure
{
    /// <summary>
    /// Inspects the action result object being returned from the controller
    /// action and sets an appropriate status code. This can be used in cases
    /// where a specific object type (such as IErrors) should always
    /// be returned with a specific Http status code.
    /// 
    /// This saves having to set the status code manually in every controller
    /// action.
    /// </summary>
    public class HttpStatusCodeConventionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is IErrors)
            {
                if (objectResult.StatusCode != null)
                {
                    throw new InvalidOperationException(
                        "An IErrors should always be returned with " +
                        "status code 400 but this response was set to status " +
                        "code " + objectResult.StatusCode);
                }

                objectResult.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
