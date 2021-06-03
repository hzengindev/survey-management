namespace Core.Utilities.Results
{
    public class ActionResult : IResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}