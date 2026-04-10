using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

public class ErrorController : Controller
{
    [Route("error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        Response.StatusCode = statusCode;

        return View("Error", statusCode);
    }
}