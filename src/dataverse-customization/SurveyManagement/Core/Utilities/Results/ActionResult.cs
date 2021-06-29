using System.Runtime.Serialization;

namespace Core.Utilities.Results
{
    [DataContract]
    public class ActionResult
    {
        [DataMember]
        public bool Success { get; set; } = true;
        [DataMember]
        public string Message { get; set; }
        [DataMember]
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