using ExaminationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public class InProgressState : IExamState
    {
        public void StartExam(ExamAttempt exam)
            => throw new InvalidOperationException("Đã bắt đầu");

        public void Submit(ExamAttempt exam)
        {
            exam.ChangeState(Enums.ExamStateType.Submitted);
        }

        public void Grade(ExamAttempt exam)
            => throw new InvalidOperationException("Chưa nộp bài");
    }

}
