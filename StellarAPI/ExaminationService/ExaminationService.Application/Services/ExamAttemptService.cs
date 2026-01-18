using System;
using ExaminationService.Application.Interfaces;
using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Entities;
using ExaminationService.Domain.Services.Persistence;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace ExaminationService.Application.Services
{
    public class ExamAttemptService : IExamAttemptService
    {
        private readonly ExamAttemptPersistence _repository;

        public ExamAttemptService(ExamAttemptPersistence repository)
        {
            _repository = repository;
        }

        public ICrudPersistence<ExamAttempt, Guid> GetCrudPersistence()
        {
            return _repository;
        }

        public IGetAllPersistence<ExamAttempt> GetGetAllPersistence()
        {
             return (IGetAllPersistence<ExamAttempt>)_repository; 
        }

        public void ValidateCreate(HeaderContext context, ExamAttempt entity, ExamAttempt request)
        {
             //Validators.ExamAttemptsValidator.ValidateCreate(context, entity, request);
        }

        public void ValidateUpdateRequest(HeaderContext context, Guid id, ExamAttempt entity, ExamAttempt request)
        {
             //Validators.ExamAttemptsValidator.ValidateUpdate(context, id, entity, request);
        }


    }
}
