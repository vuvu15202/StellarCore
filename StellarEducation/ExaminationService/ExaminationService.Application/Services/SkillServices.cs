using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Enums;

namespace ExaminationService.Application.Services
{
    public class ReadingExamService : IExamService
    {
        public Skill SupportedSkill => Skill.READING;

        public async Task<object> GetExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Skill = SupportedSkill, ExamId = examId });
            return new { Message = "Reading Exam Data", ExamId = examId, Type = strategy.Type };
        }

        public async Task<object> StartExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Start", Skill = SupportedSkill });
            return new { Message = "Reading Exam Started", StartTime = DateTime.UtcNow };
        }

        public async Task<object> SubmitExamAsync(Guid examId, object submission, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Submit", Skill = SupportedSkill });
            return new { Message = "Reading Exam Submitted", Score = 100 };
        }
    }

    public class ListeningExamService : IExamService
    {
        public Skill SupportedSkill => Skill.LISTENING;

        public async Task<object> GetExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Skill = SupportedSkill, ExamId = examId });
            return new { Message = "Listening Exam Data", ExamId = examId, Type = strategy.Type };
        }

        public async Task<object> StartExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Start", Skill = SupportedSkill });
            return new { Message = "Listening Exam Started", StartTime = DateTime.UtcNow };
        }

        public async Task<object> SubmitExamAsync(Guid examId, object submission, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Submit", Skill = SupportedSkill });
            return new { Message = "Listening Exam Submitted", Score = 90 };
        }
    }

    public class WritingExamService : IExamService
    {
        public Skill SupportedSkill => Skill.WRITING;

        public async Task<object> GetExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Skill = SupportedSkill, ExamId = examId });
            return new { Message = "Writing Exam Data", ExamId = examId, Type = strategy.Type };
        }

        public async Task<object> StartExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Start", Skill = SupportedSkill });
            return new { Message = "Writing Exam Started", StartTime = DateTime.UtcNow };
        }

        public async Task<object> SubmitExamAsync(Guid examId, object submission, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Submit", Skill = SupportedSkill });
            return new { Message = "Writing Exam Submitted", Feedback = "Good job" };
        }
    }

    public class SpeakingExamService : IExamService
    {
        public Skill SupportedSkill => Skill.SPEAKING;

        public async Task<object> GetExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Skill = SupportedSkill, ExamId = examId });
            return new { Message = "Speaking Exam Data", ExamId = examId, Type = strategy.Type };
        }

        public async Task<object> StartExamAsync(Guid examId, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Start", Skill = SupportedSkill });
            return new { Message = "Speaking Exam Started", StartTime = DateTime.UtcNow };
        }

        public async Task<object> SubmitExamAsync(Guid examId, object submission, IExamTypeStrategy strategy)
        {
            await strategy.ProcessRulesAsync(new { Action = "Submit", Skill = SupportedSkill });
            return new { Message = "Speaking Exam Submitted", Status = "Evaluating" };
        }
    }
}
