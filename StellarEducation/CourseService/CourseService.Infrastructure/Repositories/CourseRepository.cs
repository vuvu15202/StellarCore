using System;
using CourseService.Domain.Entities;
using CourseService.Domain.Interfaces;
using CourseService.Infrastructure.Database;
using Stellar.Shared.Repositories;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : CrudRepository<Course, Guid>, ICourseRepository
    {
        public CourseRepository(StellarDbContext context) : base(context)
        {
        }

        public Course getCourseTest()
        {
            return DbSet.FirstOrDefault();
            //return Query().OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            //return Context.Set<Course>().Find(Guid.Parse("...ID..."));
        }
    }
}
