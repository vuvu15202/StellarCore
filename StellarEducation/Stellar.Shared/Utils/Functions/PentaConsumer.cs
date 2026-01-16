using System;

namespace Stellar.Shared.Utils.Functions
{
    /// <summary>
    /// Delegate for actions with 5 parameters.
    /// </summary>
    /// <typeparam name="A">First parameter type</typeparam>
    /// <typeparam name="B">Second parameter type</typeparam>
    /// <typeparam name="C">Third parameter type</typeparam>
    /// <typeparam name="D">Fourth parameter type</typeparam>
    /// <typeparam name="E">Fifth parameter type</typeparam>
    public delegate void PentaConsumer<in A, in B, in C, in D, in E>(A a, B b, C c, D d, E e);

    /// <summary>
    /// Extension methods for PentaConsumer.
    /// </summary>
    public static class PentaConsumerExtensions
    {
        /// <summary>
        /// Chain another action after this one.
        /// </summary>
        public static PentaConsumer<A, B, C, D, E> AndThen<A, B, C, D, E>(
            this PentaConsumer<A, B, C, D, E> first,
            PentaConsumer<A, B, C, D, E> after)
        {
            return (a, b, c, d, e) =>
            {
                first(a, b, c, d, e);
                after(a, b, c, d, e);
            };
        }
    }
}
