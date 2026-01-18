using ExaminationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public class DraftState : IExamState
    {
        public void StartExam(ExamAttempt exam)
        {
            exam.ChangeState(Enums.ExamStateType.InProgress);
        }

        public void Submit(ExamAttempt exam)
            => throw new InvalidOperationException("Chưa mở bài thi");

        public void Grade(ExamAttempt exam)
            => throw new InvalidOperationException("Không thể chấm");
    }

}
