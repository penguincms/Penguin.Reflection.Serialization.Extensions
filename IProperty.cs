using Penguin.Reflection.Serialization.Abstractions.Interfaces;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class IMetaPropertyExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Gets the value of the property from the speficied source casted to specified type
        /// </summary>
        /// <typeparam name="T">The type to cast the return as</typeparam>
        /// <param name="p">The IMetaProperty defining the value to be returned</param>
        /// <param name="target">The source object</param>
        /// <returns>The source property casted to the specified type</returns>
        public static T GetValue<T>(this IMetaProperty p, IMetaObject target) => target.GetValue<T>(p.Name);

        /// <summary>
        /// Gets the value of the property from the speficied source as a string
        /// </summary>
        /// <param name="p">The IMetaProperty defining the value to be returned</param>
        /// <param name="target">The source object</param>
        /// <returns>The value of the property from the speficied source as a string</returns>
        public static string GetValue(this IMetaProperty p, IMetaObject target) => target[p.Name].Value;

        #endregion Methods
    }
}