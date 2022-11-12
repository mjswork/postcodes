using System.Net;

namespace Postcodes.Models
{
    public class ErrorResult
    {
        public ErrorResult(int status, string error, string message)
        {
            Message= message;
            Status = status;
            Error = error;
        }

        public ErrorResult(string message)
        {
            Message = message;
        }

        public ErrorResult() {}

        public string Message { get; set; } = "Something went wrong";
        public int Status { get; set; } = (int)HttpStatusCode.InternalServerError;
        public string Error { get; set; } = "InternalServerError";
    }
}
