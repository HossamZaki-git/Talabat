namespace Talabat.WebAPI.Errors
{
    public class ErrorReturn
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public ErrorReturn(int StatusCode, string? Message = null)
        {
            this.StatusCode = StatusCode;
            string defaultMessage = StatusCode switch
            {
                400 => "You have made a bad request",
                401 => "You have made an unauthorized request",
                403 => "Your request is forbidden",
                404 => "Not found",
                408 => "Request timeout",
                500 => "Internal server error, we will work to solve it as soon as possible",
                _ => "An error has happened"
            };
            this.Message = Message ?? defaultMessage;
        }
    }
}
