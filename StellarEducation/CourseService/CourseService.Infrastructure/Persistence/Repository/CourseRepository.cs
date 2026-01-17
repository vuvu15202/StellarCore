using System;
using CourseService.Domain.Models.Entities;
using CourseService.Domain.Services.Persistence;
using CourseService.Infrastructure.Database;
using Stellar.Shared.Repositories;

namespace CourseService.Infrastructure.Persistence.Repository
{
    public class CourseRepository : CrudRepository<Course, Guid>, CoursePersistence
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
