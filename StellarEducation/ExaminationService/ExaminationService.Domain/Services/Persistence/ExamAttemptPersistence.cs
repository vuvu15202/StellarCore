using System;
using ExaminationService.Domain.Models.Entities;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;

namespace ExaminationService.Domain.Services.Persistence
{
    public interface ExamAttemptPersistence : ICrudPersistence<ExamAttempt, Guid>
    {
    }
}
