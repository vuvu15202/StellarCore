using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Interfaces
{
    public interface IExamService1
    {
        void Start(Guid attemptId);
        void Submit(Guid attemptId);
        void Grade(Guid attemptId);
    }

}
