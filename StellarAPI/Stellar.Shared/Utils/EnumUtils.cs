using System;
using Stellar.Shared.Models.Enums;
using Stellar.Shared.Models.Exceptions;
using System.Collections.Generic;

namespace Stellar.Shared.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Convert a string value to an enum.
        /// </summary>
        /// <param name="enumType">The enum type</param>
        /// <param name="value">The value to convert</param>
        /// <returns>Enum value</returns>
        /// <exception cref="ResponseException">Thrown when conversion fails</exception>
        public static object ConvertEnum(Type enumType, object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null!;
            }

            try
            {
                if (enumType.IsEnum)
                {
                    return Enum.Parse(enumType, value.ToString()!, true);
                }
            }
            catch
            {
                // Fall through to exception
            }

            throw new ResponseException(
                $"Invalid enum value '{value}' for type '{enumType.Name}'",
                CommonErrorMessage.ENUM_FAILED,
                400);
        }

        /// <summary>
        /// Convert a byte value to an enum that implements IEnum.
        /// </summary>
        /// <typeparam name="E">Enum type that implements IEnum</typeparam>
        /// <param name="value">Byte value</param>
        /// <returns>Enum value</returns>
        /// <exception cref="ResponseException">Thrown when no matching enum is found</exception>
        public static E FromValue<E>(byte value) where E : struct, Enum, IEnum
        {
            foreach (E enumValue in Enum.GetValues<E>())
            {
                if (enumValue.GetValue() == value)
                {
                    return enumValue;
                }
            }

            throw new ResponseException(
                $"Invalid enum value '{value}' for type '{typeof(E).Name}'",
                CommonErrorMessage.ENUM_FAILED,
                400);
        }
    }
}
