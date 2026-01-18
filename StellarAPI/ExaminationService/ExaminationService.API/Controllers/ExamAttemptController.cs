using ExaminationService.Application.Interfaces;
using ExaminationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;

namespace ExaminationService.APIs
{
    public class ExamAttemptsController : BaseApi<ExamAttempt, Guid, ExamAttempt, ExamAttempt, ExamAttempt>
    {
        private readonly IExamAttemptService _service;

        public ExamAttemptsController(IExamAttemptService service)
        {
            _service = service;
        }

        protected override IBaseService<ExamAttempt, Guid, ExamAttempt, ExamAttempt, ExamAttempt> Service => (IBaseService<ExamAttempt, Guid, ExamAttempt, ExamAttempt, ExamAttempt>)_service;
    }
}
