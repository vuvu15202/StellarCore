using System;
using ExaminationService.Domain.Models.Entities;
using ExaminationService.Infrastructure.Persistence;
using Stellar.Shared.Services;

namespace ExaminationService.Application.Interfaces
{
    public interface IExamAttemptService : IBaseService<ExamAttempt, Guid, ExamAttempt, ExamAttempt, ExamAttempt>
    {

    }
}
