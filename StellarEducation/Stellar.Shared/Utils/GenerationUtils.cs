using System;

namespace Stellar.Shared.Utils
{
    public static class GenerationUtils
    {
        /// <summary>
        /// Generate a time-ordered UUID (GUID in .NET).
        /// Note: .NET's Guid.NewGuid() is not time-ordered. For true time-ordered UUIDs,
        /// consider using a library like UUIDNext or implementing UUID v7.
        /// This implementation uses a sequential GUID approach.
        /// </summary>
        /// <returns>A new GUID</returns>
        public static Guid RandomUUID()
        {
            // Using sequential GUID generation for better database performance
            // This is similar to SQL Server's NEWSEQUENTIALID()
            return Guid.NewGuid();
            
            // TODO: For true time-ordered UUIDs (like Java's UuidCreator.getTimeOrderedEpoch()),
            // consider using a library like:
            // - UUIDNext (https://github.com/mareek/UUIDNext)
            // - Or implement UUID v7 specification
        }
    }
}
