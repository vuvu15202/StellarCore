using System;
using CourseService.Domain.Entities;
using Stellar.Shared.Interfaces;

namespace CourseService.Domain.Interfaces
{
    public interface ICourseRepository : ICrudPersistence<Course, Guid>
    {
        Course getCourseTest();

    }
}
