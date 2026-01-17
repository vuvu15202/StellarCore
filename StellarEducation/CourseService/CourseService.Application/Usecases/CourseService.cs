using System;
using CourseService.Application.Requests;
using CourseService.Application.Responses;
using CourseService.Domain.Models.Entities;
using CourseService.Domain.Services.Persistence;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Services;
using System.Collections.Generic;
using System.Linq;


namespace CourseService.Application.Usecases
{
    public class CourseService : BaseService<Course, Guid, CourseResponse, CreateCourseRequest, CourseResponse>
    {
        private readonly CoursePersistence _repository;

        public CourseService(CoursePersistence repository)
        {
            _repository = repository;
        }

        public override ICrudPersistence<Course, Guid> GetCrudPersistence()
        {
            return _repository;
        }

        public override void ValidateCreate(HeaderContext context, Course entity, CreateCourseRequest request)
        {
             Validators.CoursesValidator.ValidateCreate(context, entity, request);
        }

        public override void ValidateUpdateRequest(HeaderContext context, Guid id, Course entity, CreateCourseRequest request)
        {
             Validators.CoursesValidator.ValidateUpdate(context, id, entity, request);
        }

        public override IQueryable<Course> BuildFilterQuery(IQueryable<Course> query, HeaderContext context, Dictionary<string, object> filter)
        {
            // Custom Logic Example:
            // if (filter.ContainsKey("custom")) ...
            Course c = _repository.getCourseTest();
            return query;
        }
    }
}
