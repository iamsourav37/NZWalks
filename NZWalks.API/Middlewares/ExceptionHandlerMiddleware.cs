using NZWalks.API.Utility;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception occured, details: {ex.Message}");
                ApiResponse _response = new()
                {
                    IsSuccess = false,
                    Data = null,
                    Errors = ["Internal Server Error", $"Exception Details: {ex.Message}"]
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(_response);
            }
        }
    }
}
