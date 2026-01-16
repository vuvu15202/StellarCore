using System;
using CourseService.Application.Interfaces;
using CourseService.Application.Requests;
using CourseService.Domain.Entities;
using CourseService.Domain.Interfaces;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace CourseService.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        public ICrudPersistence<Course, Guid> GetCrudPersistence()
        {
            return _repository;
        }

        public IGetAllPersistence<Course> GetGetAllPersistence()
        {
             return (IGetAllPersistence<Course>)_repository; 
        }

        public void ValidateCreate(HeaderContext context, Course entity, CreateCourseRequest request)
        {
             Validators.CoursesValidator.ValidateCreate(context, entity, request);
        }

        public void ValidateUpdateRequest(HeaderContext context, Guid id, Course entity, CreateCourseRequest request)
        {
             Validators.CoursesValidator.ValidateUpdate(context, id, entity, request);
        }

        public IQueryable<Course> BuildFilterQuery(IQueryable<Course> query, HeaderContext context, Dictionary<string, object> filter)
        {
            // Custom Logic Example:
            // if (filter.ContainsKey("custom")) ...
            Course c = _repository.getCourseTest();
            return query;
        }


    }
}
