using System;

namespace Stellar.Shared.Utils.Functions
{
    /// <summary>
    /// Delegate for actions with 4 parameters.
    /// </summary>
    /// <typeparam name="A">First parameter type</typeparam>
    /// <typeparam name="B">Second parameter type</typeparam>
    /// <typeparam name="C">Third parameter type</typeparam>
    /// <typeparam name="D">Fourth parameter type</typeparam>
    public delegate void QuadConsumer<in A, in B, in C, in D>(A a, B b, C c, D d);

    /// <summary>
    /// Extension methods for QuadConsumer.
    /// </summary>
    public static class QuadConsumerExtensions
    {
        /// <summary>
        /// Chain another action after this one.
        /// </summary>
        public static QuadConsumer<A, B, C, D> AndThen<A, B, C, D>(
            this QuadConsumer<A, B, C, D> first,
            QuadConsumer<A, B, C, D> after)
        {
            return (a, b, c, d) =>
            {
                first(a, b, c, d);
                after(a, b, c, d);
            };
        }
    }
}
