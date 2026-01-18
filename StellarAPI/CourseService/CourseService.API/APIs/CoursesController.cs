using Microsoft.AspNetCore.Mvc;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using CourseService.Application.Requests;
using CourseService.Application.Responses;
using CourseService.Domain.Models.Entities;

namespace CourseService.APIs
{
    public class CoursesController : BaseApi<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>
    {
        private readonly CourseService.Application.Usecases.CourseService _service;

        public CoursesController(CourseService.Application.Usecases.CourseService service)
        {
            _service = service;
        }

        protected override IBaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse> Service => (IBaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>)_service;
    }
}
