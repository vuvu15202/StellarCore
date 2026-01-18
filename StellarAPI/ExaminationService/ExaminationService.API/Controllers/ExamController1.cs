using ExaminationService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationService.API.Controllers
{
    [ApiController]
    [Route("api/exams")]
    public class ExamController1 : ControllerBase
    {
        private readonly IExamService1 _service;

        public ExamController1(IExamService1 service)
        {
            _service = service;
        }

        [HttpPost("{id}/start")]
        public IActionResult Start(Guid id)
        {
            _service.Start(id);
            return Ok();
        }
    }

}
