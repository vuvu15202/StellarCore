using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Enums;

namespace ExaminationService.Application.Factories
{
    public interface IExamServiceFactory
    {
        IExamService GetService(Skill skill);
    }

    public interface IExamTypeStrategyFactory
    {
        IExamTypeStrategy GetStrategy(ExamType type);
    }

    public class ExamServiceFactory : IExamServiceFactory
    {
        private readonly IEnumerable<IExamService> _services;

        public ExamServiceFactory(IEnumerable<IExamService> services)
        {
            _services = services;
        }

        public IExamService GetService(Skill skill)
        {
            var service = _services.FirstOrDefault(s => s.SupportedSkill == skill);
            if (service == null)
                throw new Exception($"Service for skill {skill} not found.");
            return service;
        }
    }

    public class ExamTypeStrategyFactory : IExamTypeStrategyFactory
    {
        private readonly IEnumerable<IExamTypeStrategy> _strategies;

        public ExamTypeStrategyFactory(IEnumerable<IExamTypeStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IExamTypeStrategy GetStrategy(ExamType type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.Type == type);
            if (strategy == null)
                throw new Exception($"Strategy for type {type} not found.");
            return strategy;
        }
    }
}
