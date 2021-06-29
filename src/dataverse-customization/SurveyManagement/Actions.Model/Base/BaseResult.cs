using Core.Utilities.Results;
using Core.Utilities.Serialize;

namespace Actions.Models.Base
{
    public class BaseResult
    {
        public ActionResult ActionResult { get; set; }
        public string ActionResultSerialize { get => ActionResult == null ? string.Empty : Serializer.Serialize<ActionResult>(ActionResult); }

        public BaseResult(bool success)
        {
            this.ActionResult = new ActionResult(success);
        }

        public BaseResult(bool success, string message)
        {
            this.ActionResult = new ActionResult(success, message);
        }

        public BaseResult(bool success, string message, int code)
        {
            this.ActionResult = new ActionResult(success, message, code);
        }
    }
}