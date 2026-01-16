using ExaminationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public class GradedState : IExamState
    {
        public void StartExam(ExamAttempt exam)
            => throw new InvalidOperationException("Đã chấm");

        public void Submit(ExamAttempt exam)
            => throw new InvalidOperationException("Đã chấm");

        public void Grade(ExamAttempt exam)
            => throw new InvalidOperationException("Đã chấm rồi");
    }

}
