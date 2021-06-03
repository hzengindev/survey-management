using Business.Abstract.Survey;
using DTOs.Survey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("survey")]
    public class SurveyController : ApiController
    {
        private readonly ILogger<SurveyController> _logger;
        private readonly IGetSurvey _getSurvey;
        private readonly ICompleteSurvey _completeSurvey;

        public SurveyController(ILogger<SurveyController> logger, IGetSurvey getSurvey, ICompleteSurvey completeSurvey)
        {
            _logger = logger;
            _getSurvey = getSurvey;
            _completeSurvey = completeSurvey;
        }

        [HttpGet("{code}")]
        public IActionResult GetSurvey([FromRoute]string code)
        {
            var serviceResult = _getSurvey.Handle(new GetSurveyHandlerInput(code));

            if (!serviceResult.Success)
                return Error(serviceResult.Message, serviceResult.Code);

            return Success(serviceResult.Data, serviceResult.Message);
        }

        [HttpPost("complete")]
        public IActionResult Complete([FromBody] CompleteSurveyHandlerInput input)
        {
            //TODO: sanitize / encoding / security
            var serviceResult = _completeSurvey.Handle(input);

            if (!serviceResult.Success)
                return Error(serviceResult.Message, serviceResult.Code);

            return Success(serviceResult.Message);
        }
    }
}
