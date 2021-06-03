using Business.Abstract.Image;
using Core.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("image")]
    public class ImageController : ApiController
    {
        private readonly ILogger<SurveyController> _logger;
        private readonly IGetImage _getImage;

        public ImageController(ILogger<SurveyController> logger, IGetImage getImage)
        {
            _logger = logger;
            _getImage = getImage;
        }

        [ResponseCache(VaryByHeader = "User-Agent", Duration = 86400)]
        [HttpGet("{type}/{id}")]
        public IActionResult GetImage([FromRoute]ImageType type, [FromRoute]Guid id)
        {
            var serviceResult = _getImage.Handle(new DTOs.Image.GetImageHandlerInput(type, id));

            if (!serviceResult.Success)
                return NotFound();

            if (!string.IsNullOrEmpty(serviceResult.Data.File.Filename))
                return File(serviceResult.Data.File.File, serviceResult.Data.File.Mimetype, serviceResult.Data.File.Filename);

            return File(serviceResult.Data.File.File, serviceResult.Data.File.Mimetype);
        }
    }
}
