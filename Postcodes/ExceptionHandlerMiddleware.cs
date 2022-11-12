using Newtonsoft.Json;
using Postcodes.Models;
using System.Net;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

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
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var errorModel = new ErrorResult(error.Message);

            switch (error)
            {
                case KeyNotFoundException e:
                    errorModel.Status = (int)HttpStatusCode.NotFound;
                    errorModel.Error = HttpStatusCode.NotFound.ToString();
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    errorModel.Status = (int)HttpStatusCode.InternalServerError;
                    errorModel.Error = HttpStatusCode.InternalServerError.ToString();
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonConvert.SerializeObject(errorModel);
            await response.WriteAsync(result);
        }
    }
}
