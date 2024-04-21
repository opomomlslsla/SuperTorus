using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Data.Common;
using System.Net;
using System.Text.Json;

namespace SuperTorus.Application.Middleware
{
    public class ExeptionHandlerMiddleware(RequestDelegate requestDelegate)
    {
        private RequestDelegate _requestDelegate = requestDelegate;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (ValidationException ex)
            {
                Log.Information($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace} \n");

                string msg = "Your data is incorrect, every field must be greater than 0 and MaxRadius must be greater than MinRadius";

                await HandleExeptionAsync(httpContext, msg, HttpStatusCode.BadRequest);

            }
            catch(DbException ex)
            {
                Log.Information($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace} \n");

                string msg = $"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace} \n";

                await HandleExeptionAsync(httpContext, msg, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace} \n");

                string msg = "Internal server error";

                await HandleExeptionAsync(httpContext, msg, HttpStatusCode.BadRequest);

            }
        }

        private async Task HandleExeptionAsync(HttpContext httpContext, string msg, HttpStatusCode statusCode)
        {
            HttpResponse response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;
            await response.WriteAsJsonAsync(JsonSerializer.Serialize(msg));
        }

    }
}
