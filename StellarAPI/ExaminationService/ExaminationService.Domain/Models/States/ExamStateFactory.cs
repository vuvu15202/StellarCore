using ExaminationService.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.States
{
    public static class ExamStateFactory
    {
        public static IExamState Create(ExamStateType type)
        {
            return type switch
            {
                ExamStateType.Draft => new DraftState(),
                ExamStateType.InProgress => new InProgressState(),
                ExamStateType.Submitted => new SubmittedState(),
                ExamStateType.Graded => new GradedState(),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }
}
