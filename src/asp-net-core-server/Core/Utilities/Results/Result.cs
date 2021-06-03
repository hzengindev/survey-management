namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message, int code)
        {
            this.Success = success;
            this.Message = message;
            this.Code = code;
        }

        public Result(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public Result(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}