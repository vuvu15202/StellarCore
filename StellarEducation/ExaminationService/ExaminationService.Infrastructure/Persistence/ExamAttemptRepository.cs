using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Entities;
using ExaminationService.Domain.Services.Persistence;
using ExaminationService.Infrastructure.Database;
using Stellar.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationService.Infrastructure.Persistence
{
    public class ExamAttemptRepository : CrudRepository<ExamAttempt, Guid>, ExamAttemptPersistence
    {
        public ExamAttemptRepository(StellarDbContext context) : base(context)
        {
        }

        public new ExamAttempt? FindById(Guid id)
        {
            var attempt = base.FindById(id);
            attempt?.LoadState();
            return attempt;
        }
    }
}
