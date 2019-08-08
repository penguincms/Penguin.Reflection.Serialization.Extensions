using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using System;
using System.Linq;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class IHasAttributesExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Returns an attribute instance of a specified type
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check for</typeparam>
        /// <param name="o">Type type of the object to check for attributes</param>
        /// <returns>The attribute, or null if not found</returns>
        public static IMetaObject Attribute<T>(this IHasAttributes o) => Attribute(o, typeof(T));

        /// <summary>
        /// Returns an attribute instance of a specified type
        /// </summary>
        /// <param name="o">Type type of the object to check for attributes</param>
        /// <param name="t">The type of the attribute to check for</param>
        /// <returns>The attribute, or null if not found</returns>
        public static IMetaObject Attribute(this IHasAttributes o, Type t) => o.Attributes.FirstOrDefault(a => a.Type.FullName == t.FullName)?.Instance;

        /// <summary>
        /// Gets an attribute of a given type and returns a nullable? of its value
        /// </summary>
        /// <typeparam name="X">The type of the attribute</typeparam>
        /// <typeparam name="Y">The type of the value to return</typeparam>
        /// <param name="o">The object to check</param>
        /// <param name="PropertyName">The property name to look for</param>
        /// <returns>A nullable struct representation of its value</returns>
        public static Y? AttributeNullable<X, Y>(this IHasAttributes o, string PropertyName) where Y : struct
        {
            if (!o.HasAttribute<X>() || !o.Attribute<X>().HasProperty(PropertyName))
            {
                return null;
            }

            return o.Attribute<X>().GetProperty(PropertyName).GetValue<Y>();
        }

        /// <summary>
        /// Gets an attribute of a given type and then casts and returns a specified value
        /// </summary>
        /// <typeparam name="X">The type of the attribute</typeparam>
        /// <typeparam name="Y">The type of the value to return</typeparam>
        /// <param name="o">The object to check</param>
        /// <param name="PropertyName">The property name to look for</param>
        /// <returns>Either the casted property or null</returns>
        public static Y AttributeRef<X, Y>(this IHasAttributes o, string PropertyName) where Y : class
        {
            if (!o.HasAttribute<X>() || !o.Attribute<X>().HasProperty(PropertyName))
            {
                return null;
            }

            return o.Attribute<X>().GetProperty(PropertyName).GetValue<Y>();
        }

        /// <summary>
        /// Gets an attribute of a given type and then casts and returns a specified value
        /// </summary>
        /// <typeparam name="X">The type of the attribute</typeparam>
        /// <typeparam name="Y">The type of the value to return</typeparam>
        /// <param name="o">The object to check</param>
        /// <param name="PropertyName">The property name to look for</param>
        /// <param name="Default">Default to return if property or attribute does not exist</param>
        /// <returns>Either the casted property, or default</returns>
        public static Y AttributeRef<X, Y>(this IHasAttributes o, string PropertyName, Y Default) where Y : class
        {
            if (!o.HasAttribute<X>() || !o.Attribute<X>().HasProperty(PropertyName))
            {
                return Default;
            }

            return o.Attribute<X>().GetProperty(PropertyName).GetValue<Y>() ?? Default;
        }

        /// <summary>
        /// Gets an attribute of a given type and then casts and returns a specified value
        /// </summary>
        /// <typeparam name="X">The type of the attribute</typeparam>
        /// <typeparam name="Y">The type of the value to return</typeparam>
        /// <param name="o">The object to check</param>
        /// <param name="PropertyName">The property name to look for</param>
        /// <param name="Default">Default to return if property or attribute does not exist</param>
        /// <returns>Either the casted property, or default</returns>
        public static Y AttributeStruct<X, Y>(this IHasAttributes o, string PropertyName, Y Default) where Y : struct
        {
            if (!o.HasAttribute<X>() || !o.Attribute<X>().HasProperty(PropertyName))
            {
                return Default;
            }

            return o.Attribute<X>().GetProperty(PropertyName).GetValue<Y>();
        }

        /// <summary>
        /// Retrieves a property value of an attribute of a given type (non-reference)
        /// </summary>
        /// <typeparam name="Y">The type of the property to return</typeparam>
        /// <param name="o">The object to check</param>
        /// <param name="t">The type of the attribute to search for</param>
        /// <param name="PropertyName">The name of the property to retrieve the value for</param>
        /// <param name="Default">If the property is not found, this is the default to return in place of null</param>
        /// <returns>Either the casted property, or default</returns>
        public static Y AttributeStruct<Y>(this IHasAttributes o, Type t, string PropertyName, Y Default) where Y : struct
        {
            if (!o.HasAttribute(t) || !o.Attribute(t).HasProperty(PropertyName))
            {
                return Default;
            }

            return o.Attribute(t).GetProperty(PropertyName).GetValue<Y>();
        }

        /// <summary>
        /// Gets an attribute of a given type and then casts and returns a specified value
        /// </summary>
        /// <typeparam name="X">The type of the attribute to retrieve</typeparam>
        /// <typeparam name="Y">The type of the value to return</typeparam>
        /// <param name="o">The source of the attribute</param>
        /// <param name="PropertyName">The name of the property on the attribute to retrieve</param>
        /// <returns>The casted property value found on the attribute</returns>
        public static Y GetAttributeValue<X, Y>(this IHasAttributes o, string PropertyName) => o.Attribute<X>()[PropertyName].GetValue<Y>();

        /// <summary>
        /// Checks to see if the object contains an attribute of a given type (by FullName)
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check for</typeparam>
        /// <param name="o">The object to check</param>
        /// <returns>A bool indicating whether or not the attribute was found</returns>
        public static bool HasAttribute<T>(this IHasAttributes o) => HasAttribute(o, typeof(T));

        /// <summary>
        /// Checks to see if the object contains an attribute of a given type (by FullName)
        /// </summary>
        /// <param name="o">The object to check</param>
        /// <param name="t">The type of the attribute to check for</param>
        /// <returns>A bool indicating whether or not the attribute was found</returns>
        public static bool HasAttribute(this IHasAttributes o, Type t) => o?.Attributes != null && o.Attributes.Any(a => a.Type.FullName == t.FullName);

        #endregion Methods
    }
}