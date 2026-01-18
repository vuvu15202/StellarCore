using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Enums;

namespace ExaminationService.Application.Strategies
{
    public class TestStrategy : IExamTypeStrategy
    {
        public ExamType Type => ExamType.TEST;

        public async Task ProcessRulesAsync(object context)
        {
            // Logic for TEST type (e.g., set time limits, disable backtracking)
            await Task.CompletedTask;
        }
    }

    public class HomeworkStrategy : IExamTypeStrategy
    {
        public ExamType Type => ExamType.HOMEWORK;

        public async Task ProcessRulesAsync(object context)
        {
            // Logic for HOMEWORK type (e.g., allow multiple attempts)
            await Task.CompletedTask;
        }
    }
}
