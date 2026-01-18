using System;
using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Models.Entities;
using ExaminationService.Domain.Services.Persistence;
using ExaminationService.Infrastructure.Database;
using Stellar.Shared.Repositories;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : CrudRepository<ExamAttempt, Guid>, ExamAttemptPersistence
    {
        public CourseRepository(StellarDbContext context) : base(context)
        {
        }
    }
}
