using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Domain.Models.Enums
{
    public enum ExamStateType
    {
        Draft = 0,
        InProgress = 1,
        Submitted = 2,
        Graded = 3,
        Expired = 4
    }
}
