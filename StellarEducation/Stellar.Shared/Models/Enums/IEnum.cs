namespace Stellar.Shared.Models.Enums
{
    /// <summary>
    /// Interface for enums that have a byte value.
    /// </summary>
    public interface IEnum
    {
        byte GetValue();
    }
}
