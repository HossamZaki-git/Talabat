namespace Talabat.WebAPI.Errors
{
    public class ValidationErrorReturn : ErrorReturn
    {
        public List<string> modelStateErrors { get; set; }
        public ValidationErrorReturn(List<string> modelStateErrors):base(400)
        {
            this.modelStateErrors = modelStateErrors;
        }
    }
}
