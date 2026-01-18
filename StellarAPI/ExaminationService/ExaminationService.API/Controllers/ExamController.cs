using ExaminationService.Application.Factories;
using ExaminationService.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamServiceFactory _serviceFactory;
        private readonly IExamTypeStrategyFactory _strategyFactory;

        public ExamController(IExamServiceFactory serviceFactory, IExamTypeStrategyFactory strategyFactory)
        {
            _serviceFactory = serviceFactory;
            _strategyFactory = strategyFactory;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExam(Guid id, Skill skill, ExamType type)
        {
            var service = _serviceFactory.GetService(skill);
            var strategy = _strategyFactory.GetStrategy(type);
            var result = await service.GetExamAsync(id, strategy);
            return Ok(result);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartExam(Guid id, Skill skill, ExamType type)
        {
            var service = _serviceFactory.GetService(skill);
            var strategy = _strategyFactory.GetStrategy(type);
            var result = await service.StartExamAsync(id, strategy);
            return Ok(result);
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitExam(Guid id, [FromBody] object submission, Skill skill, ExamType type)
        {
            var service = _serviceFactory.GetService(skill);
            var strategy = _strategyFactory.GetStrategy(type);
            var result = await service.SubmitExamAsync(id, submission, strategy);
            return Ok(result);
        }
    }
}
