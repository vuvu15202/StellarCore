using System;
using CourseService.Application.Requests;
using CourseService.Application.Responses;
using CourseService.Domain.Entities;
using Stellar.Shared.Services;

namespace CourseService.Application.Interfaces
{
    public interface ICourseService : IBaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>
    {

    }
}
