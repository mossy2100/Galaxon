using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Development.Application;

public static class ControllerBaseExtensions
{
    /// <inheritdoc cref="ControllerBase.StatusCode(int)"/>
    /// <summary>
    /// Overload that permits HttpStatusCode values to be used for the statusCode parameter.
    /// </summary>
    public static StatusCodeResult StatusCode(this ControllerBase controllerBase, HttpStatusCode statusCode)
    {
        return controllerBase.StatusCode((int)statusCode);
    }

    /// <inheritdoc cref="ControllerBase.StatusCode(int, object)"/>
    /// <summary>
    /// Overload that permits HttpStatusCode values to be used for the statusCode parameter.
    /// </summary>
    public static ObjectResult StatusCode(this ControllerBase controllerBase, HttpStatusCode statusCode, object? value)
    {
        return controllerBase.StatusCode((int)statusCode, value);
    }
}
