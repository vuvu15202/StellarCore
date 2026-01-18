using System;
using CourseService.Domain.Models.Entities;
using Stellar.Shared.Interfaces.Persistence;

namespace CourseService.Domain.Services.Persistence
{
    public interface CoursePersistence : ICrudPersistence<Course, Guid>
    {
        Course getCourseTest();

    }
}
