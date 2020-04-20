using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TemplateProductName.WebApi.Extensions
{
    public static class ControllerExtensions
    {
        public static JsonResult Json(this Controller controller, object data, HttpStatusCode httpStatusCode)
        {
            controller.Response.StatusCode = (int)httpStatusCode;
            return controller.Json(data);
        }

        public static JsonResult JsonBadRequest(this Controller controller, object data) =>
            Json(controller, data, HttpStatusCode.BadRequest);
    }
}
