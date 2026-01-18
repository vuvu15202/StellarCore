using Stellar.Shared.Models.Enums;

namespace Stellar.Shared.Utils
{
    public static class HashUtils
    {
        private static readonly BCrypt.Net.BCrypt BcryptEncoder = new();

        /// <summary>
        /// Encode a string using BCrypt hashing.
        /// </summary>
        /// <param name="sequence">The string to hash</param>
        /// <returns>BCrypt hashed string</returns>
        public static string EncodeBCrypt(string sequence)
        {
            return BCrypt.Net.BCrypt.HashPassword(sequence);
        }

        /// <summary>
        /// Verify a string against a BCrypt hash.
        /// </summary>
        /// <param name="sequence">The plain text string</param>
        /// <param name="hashedSequence">The BCrypt hash to verify against</param>
        /// <returns>True if the string matches the hash</returns>
        public static bool MatchBcrypt(string sequence, string hashedSequence)
        {
            return BCrypt.Net.BCrypt.Verify(sequence, hashedSequence);
        }

        /// <summary>
        /// Encode a string using the specified hash type.
        /// </summary>
        /// <param name="type">The hash type to use</param>
        /// <param name="sequence">The string to hash</param>
        /// <returns>Hashed string</returns>
        /// <exception cref="ArgumentException">Thrown for invalid hash type</exception>
        public static string Encode(HashType type, string sequence)
        {
            return type switch
            {
                HashType.BCRYPT => EncodeBCrypt(sequence),
                _ => throw new ArgumentException("Invalid hash type")
            };
        }

        /// <summary>
        /// Verify a string against a hash using the specified hash type.
        /// </summary>
        /// <param name="type">The hash type to use</param>
        /// <param name="sequence">The plain text string</param>
        /// <param name="hashedSequence">The hash to verify against</param>
        /// <returns>True if the string matches the hash</returns>
        /// <exception cref="ArgumentException">Thrown for invalid hash type</exception>
        public static bool Match(HashType type, string sequence, string hashedSequence)
        {
            return type switch
            {
                HashType.BCRYPT => MatchBcrypt(sequence, hashedSequence),
                _ => throw new ArgumentException("Invalid hash type")
            };
        }
    }
}
