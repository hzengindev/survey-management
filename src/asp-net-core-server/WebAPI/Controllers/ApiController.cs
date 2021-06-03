using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ApiController : Controller
    {
        [NonAction]
        public IActionResult Success()
        {
            return Ok(new { Success = true });
        }

        [NonAction]
        public IActionResult Success(string message)
        {
            return Ok(new { Success = true, Message = message });
        }

        [NonAction]
        public IActionResult Success(object data = default(object))
        {
            return Ok(new { Success = true, Data = data });
        }

        [NonAction]
        public IActionResult Success(object data = default(object), string message = "")
        {
            return Ok(new { Success = true, Data = data, Message = message });
        }

        [NonAction]
        public IActionResult Error(string errorMessage = default(string))
        {
            return BadRequest(new { Success = false, Message = errorMessage });
        }

        [NonAction]
        public IActionResult Error(string errorMessage = default(string), int? errorCode = (int?)null)
        {
            return BadRequest(new { Success = false, Message = errorMessage, Code = errorCode, });
        }

        [NonAction]
        public IActionResult Error(string errorMessage = default(string), int? errorCode = (int?)null, object data = default(object))
        {
            return BadRequest(new { Success = false, Message = errorMessage, Code = errorCode, Data = data });
        }
    }
}
