using Microsoft.AspNetCore.Mvc;
using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using Stellar.Shared.APIs;
using Stellar.Shared.Models;
using Stellar.Shared.Services;
using CourseService.Application.Requests;
using CourseService.Application.Responses;

namespace CourseService.APIs
{
    public class CoursesController : BaseApi<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        protected override IBaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse> Service => (IBaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>)_service;
    }
}
