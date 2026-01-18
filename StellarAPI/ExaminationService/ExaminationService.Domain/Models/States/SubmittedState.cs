using ExaminationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public class SubmittedState : IExamState
    {
        public void StartExam(ExamAttempt exam)
            => throw new InvalidOperationException("Đã nộp bài");

        public void Submit(ExamAttempt exam)
            => throw new InvalidOperationException("Đã nộp");

        public void Grade(ExamAttempt exam)
        {
            exam.ChangeState(Enums.ExamStateType.Graded);
        }
    }

}
