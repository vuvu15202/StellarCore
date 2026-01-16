using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Entities;
using ExaminationService.Domain.Services.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Application.Services
{
    public class ExamService : IExamService1
    {
        private readonly ExamAttemptPersistence _repo;

        public ExamService(ExamAttemptPersistence repo)
        {
            _repo = repo;
        }

        public Guid CreateAttempt(Guid userId, Guid examId)
        {
            var attempt = new ExamAttempt(userId, examId);
            _repo.Save(attempt);
            return attempt.Id;
        }

        public void Start(Guid attemptId)
        {
            var exam = _repo.FindById(attemptId);
            exam.StartExam();
            _repo.Save(exam);
        }

        public void Submit(Guid attemptId)
        {
            var exam = _repo.FindById(attemptId);
            exam.Submit();
            _repo.Save(exam);
        }

        public void Grade(Guid attemptId)
        {
            var exam = _repo.FindById(attemptId);
            exam.Grade();
            _repo.Save(exam);
        }
    }


}
