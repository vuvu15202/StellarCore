using ExaminationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public interface IExamState
    {
        void StartExam(ExamAttempt exam);
        void Submit(ExamAttempt exam);
        void Grade(ExamAttempt exam);
    }

}
