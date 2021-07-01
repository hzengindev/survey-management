using System.Runtime.Serialization;

namespace Core.Utilities.Results
{
    [DataContract]
    public class ActionResult
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; } = true;
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "code")]
        public int Code { get; set; }

        public ActionResult(bool success)
        {
            this.Success = success;
        }

        public ActionResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public ActionResult(bool success, string message, int code)
        {
            this.Success = success;
            this.Message = message;
            this.Code = code;
        }
    }
}