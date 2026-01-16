using System;

namespace Stellar.Shared.Models.Enums
{
    public enum SortEnum
    {
        DESC = -1,
        ASC = 1
    }

    public static class SortEnumExtensions
    {
        public static int GetOriginal(this SortEnum sortEnum)
        {
            return (int)sortEnum;
        }

        public static SortEnum FromOriginal(int original)
        {
            return original switch
            {
                -1 => SortEnum.DESC,
                1 => SortEnum.ASC,
                _ => throw new ArgumentException($"Invalid sort value: {original}")
            };
        }
    }
}
