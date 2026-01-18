using ExaminationService.Domain.Models.Enums;

namespace ExaminationService.Domain.Interfaces
{
    public interface IExamService
    {
        Skill SupportedSkill { get; }
        Task<object> GetExamAsync(Guid examId, IExamTypeStrategy strategy);
        Task<object> StartExamAsync(Guid examId, IExamTypeStrategy strategy);
        Task<object> SubmitExamAsync(Guid examId, object submission, IExamTypeStrategy strategy);
    }

    public interface IExamTypeStrategy
    {
        ExamType Type { get; }
        Task ProcessRulesAsync(object context);
    }
}
