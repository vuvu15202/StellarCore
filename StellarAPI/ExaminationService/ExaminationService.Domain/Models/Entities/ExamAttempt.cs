using ExaminationService.Domain.Models.Enums;
using ExaminationService.Domain.Models.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.Entities
{
    public class ExamAttempt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ExamId { get; set; }

        // Persisted state
        public ExamStateType StateType { get; private set; }

        // Runtime state (NOT mapped)
        private IExamState _state = null!;

        // Parameterless constructor for EF Core and Stellar.Shared
        public ExamAttempt()
        {
            // EF Core will populate properties
            // _state will be initialized via LoadState() after materialization
        }

        public ExamAttempt(Guid userId, Guid examId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ExamId = examId;
            ChangeState(ExamStateType.Draft);
        }

        // Actions

        public void StartExam() => _state.StartExam(this);
        public void Submit() => _state.Submit(this);
        public void Grade() => _state.Grade(this);

        public void ChangeState(ExamStateType type)
        {
            StateType = type;
            _state = ExamStateFactory.Create(type);
        }

        // EF hook
        public void LoadState()
        {
            _state = ExamStateFactory.Create(StateType);
        }
    }

}
