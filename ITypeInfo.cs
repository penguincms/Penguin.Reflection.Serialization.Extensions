using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Objects;
using RType = System.Type;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class ITypeInfoExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Checks for type equality
        /// </summary>
        /// <param name="o">The object to check</param>
        /// <param name="type">a Meta Type Definition to check against</param>
        /// <returns>If the object is of the requested type</returns>
        public static bool Is(this ITypeInfo o, MetaType type)
        {
            return o is null
                ? throw new System.ArgumentNullException(nameof(o))
                : type is null ? throw new System.ArgumentNullException(nameof(type)) : o.Is(type.StringValue);
        }

        /// <summary>
        /// Checks for type equality
        /// </summary>
        /// <param name="o">The object to check</param>
        /// <param name="type">a System.Type to check against</param>
        /// <returns>If the object is of the requested type</returns>
        public static bool Is(this ITypeInfo o, RType type)
        {
            return o is null
                ? throw new System.ArgumentNullException(nameof(o))
                : type is null ? throw new System.ArgumentNullException(nameof(type)) : o.Is(type.ToString());
        }

        /// <summary>
        /// Checks for type equality
        /// </summary>
        /// <typeparam name="RType">A System.Type to check against</typeparam>
        /// <param name="o">The object to check</param>
        /// <returns>If the object is of the requested type</returns>
        public static bool Is<RType>(this ITypeInfo o)
        {
            return o is null ? throw new System.ArgumentNullException(nameof(o)) : Is(o, typeof(RType));
        }

        #endregion Methods
    }
}