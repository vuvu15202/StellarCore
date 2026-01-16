namespace Stellar.Shared.Models.Enums
{
    public enum PositionType
    {
        FIRST = 0,
        LAST = -1
    }

    public static class PositionTypeExtensions
    {
        public static byte GetValue(this PositionType positionType)
        {
            return (byte)positionType;
        }
    }
}
