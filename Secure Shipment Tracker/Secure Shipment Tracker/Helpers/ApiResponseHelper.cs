using Microsoft.AspNetCore.Mvc;
using Secure_Shipment_Tracker.Common;

namespace Secure_Shipment_Tracker.Helpers
{
    public class ApiResponseHelper
    {
        public static IActionResult Success<T>(T data, string message=null)
        {
            return new OkObjectResult(new ResponseApi<T>
            {
                Result="Success",
                Data = data,
                Message = message
            });
        }
        public static IActionResult Error(string message, int statusCode = 500)
        {
            var response = new ResponseApi<Object>
            {
                Result = "Error",
                Data = null,
                Message = message
            };
            return new OkObjectResult(response) { StatusCode = statusCode };
        }
    }
}
