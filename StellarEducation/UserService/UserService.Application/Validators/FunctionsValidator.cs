using System;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;
using Stellar.Shared.Models;
using Stellar.Shared.Models.Exceptions;

namespace UserService.Application.Validators
{
    public static class FunctionsValidator
    {
        public static void ValidateCreate(HeaderContext context, Function entity, FunctionRequest request, bool codeExists)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Function code is required");
            }

            if (codeExists)
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, $"Function code '{request.Code}' already exists");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Function name is required");
            }
        }

        public static void ValidateUpdate(HeaderContext context, Guid id, Function entity, FunctionRequest request, bool codeExists)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Function code is required");
            }

            if (codeExists)
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, $"Function code '{request.Code}' already exists");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ResponseException(CommonErrorMessage.BAD_REQUEST, "Function name is required");
            }
        }
    }
}
