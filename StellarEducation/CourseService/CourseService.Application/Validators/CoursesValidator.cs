using System;
using CourseService.Application.Requests;
using CourseService.Domain.Entities;
using Stellar.Shared.Models;
using Stellar.Shared.Models.Exceptions;

namespace CourseService.Application.Validators
{
    public static class CoursesValidator
    {
        public static void ValidateCreate(HeaderContext context, Course entity, CreateCourseRequest request)
        {
            if (request.Price <= 100)
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Price must be greater than 100");
            }
        }

        public static void ValidateUpdate(HeaderContext context, Guid id, Course entity, CreateCourseRequest request)
        {
            if (request.Price <= 100)
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Price must be greater than 100");
            }
        }
    }
}
