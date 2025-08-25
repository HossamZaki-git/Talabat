namespace Talabat.WebAPI.Errors
{
    public class ExceptionErrorReturn : ErrorReturn
    {
        public string Details { get; }
        public ExceptionErrorReturn(int statusCode, string Message = null, string Details = null) : base(statusCode, Message)
        {
            this.Details = Details;
        }
    }
}
