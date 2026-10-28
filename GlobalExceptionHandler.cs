using Microsoft.Data.SqlClient;
using MovieAPI.Services.Exceptions;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace MovieAPI
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var title = "Internal Server Error";
                var message = "An unexpected error occurred.";

                switch (exception)
                {
                    case KeyNotFoundException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        title = "Resource Not Found";
                        message = exception.Message;
                        break;

                    case ActorValidationException
                         or ProducerValidationException
                         or MovieValidationException
                         or GenreValidationException
                         or ReviewValidationException
                         or SignUpValidationException:

                        statusCode = (int)HttpStatusCode.BadRequest;
                        title = "Validation Error";
                        message = exception.Message;
                        break;

                    case UnauthorizedAccessException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        title = "Access Denied";
                        message = exception.Message;
                        break;

                    case SqlException ex when ex.Number == 547:
                        statusCode = (int)HttpStatusCode.Conflict;
                        title = "Database Constraint Error";
                        message = "Cannot complete this operation because the record is being used by another record.";
                        break;


                    case SqlException:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        title = "Database Error";
                        message = "An error occurred while communicating with the database.";
                        break;

                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        title = "Internal Server Error";
                        message = exception.Message;
                        break;
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    statusCode = statusCode,
                    title = title,
                    message = message,
                    instance = context.Request.Path
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}