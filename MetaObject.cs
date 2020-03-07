﻿using Penguin.Reflection.Abstractions;
using Penguin.Reflection.Extensions;
using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class IMetaObjectExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Adds an attribute to the MetaObject
        /// </summary>
        /// <typeparam name="T">The Type of the attribute to add</typeparam>
        /// <param name="o">The object to add the attribute to</param>
        /// <param name="Property">The first property to add to the attribute instance</param>
        /// <param name="Value">The value of the property being added</param>
        public static void AddAttribute<T>(this MetaObject o, string Property, string Value)
        {
            MetaAttribute a = new MetaAttribute(-1)
            {
                Instance = new MetaObject(-1)
                {
                    Properties = new List<IMetaObject>()
                    {
                        new MetaObject(-1)
                        {
                            Property = new  MetaProperty
                            {
                                Name = Property
                            },
                            Value = Value
                        }
                    },
                },
            };

            MetaType at = new MetaType(typeof(T), a.Instance.Properties);
            a.Type = at;
            a.Instance.Type = at;

            if (o.Property is null)
            {
                MetaProperty p = new MetaProperty
                {
                    Type = at
                };
                o.Property = p;
            }

            IList<IMetaAttribute> existingAttributes = o.Property.Attributes.ToList();

            existingAttributes.Add(a);

            (o.Property as MetaProperty).Attributes = existingAttributes;
        }

        /// <summary>
        /// Returns all attributes (property and type) from the specified object
        /// </summary>
        /// <param name="o">The object to return the attributes for</param>
        /// <returns>All the attributes. All of them.</returns>
        public static List<IMetaAttribute> AllAttributes(this IMetaObject o)
        {
            List<IMetaAttribute> toReturn = new List<IMetaAttribute>();

            if (o.Property != null)
            {
                toReturn.AddRange(o.Property.Attributes);
            }

            toReturn.AddRange(o.Type.Attributes);

            return toReturn;
        }

        /// <summary>
        /// Clears all properties and collection items. This will break if you use it on an object wrapper
        /// </summary>
        /// <param name="o">The object to clear</param>
        public static void Clear(this IMetaObject o)
        {
            (o as MetaObject).Value = null;

            if (o.Properties != null && o.Properties.Any())
            {
                foreach (IMetaObject p in o.Properties)
                {
                    p.Clear();
                }
            }

            if (o.CollectionItems != null && o.CollectionItems.Any())
            {
                foreach (IMetaObject c in o.CollectionItems)
                {
                    c.Clear();
                }

                o.CollectionItems.Clear();
            }
        }

        /// <summary>
        /// Recursively gets a property by name using a "." delimited path
        /// </summary>
        /// <param name="o">The source object</param>
        /// <param name="Path">The path of the property to get</param>
        /// <returns>The MetaObject instance of the property</returns>
        public static IMetaObject GetProperty(this IMetaObject o, string Path)
        {
            IMetaObject m = o;

            foreach (string chunk in Path.Split('.'))
            {
                m = m[chunk];
            }

            return m;
        }

        /// <summary>
        /// Gets the value of the object as its originally declared type. Probably only works for structs
        /// </summary>
        /// <param name="o">The object to get the value of </param>
        /// <returns>The object value, casted to its original type</returns>
        public static object GetTypedValue(this IMetaObject o)
        {
            return o.GetValue(System.Type.GetType(o.Type.AssemblyQualifiedName));
        }

        /// <summary>
        /// Gets the casted value of an object based on its IMetaProperty
        /// </summary>
        /// <typeparam name="T">The type to cast the value as</typeparam>
        /// <param name="o">The source object</param>
        /// <param name="property">The IMetaProperty to get</param>
        /// <returns>The casted value of an object based on its IMetaProperty</returns>
        public static T GetValue<T>(this IMetaObject o, IMetaProperty property)
        {
            return GetValue<T>(o.GetProperty(property.Name));
        }

        /// <summary>
        /// Gets the casted value of a property based on its property name. Recursive using a "." delimited path
        /// </summary>
        /// <typeparam name="T">The type to cast the value as</typeparam>
        /// <param name="o">The source object</param>
        /// <param name="PropertyName">The name of the property to get</param>
        /// <returns>The casted value of an object based on its IMetaProperty</returns>
        public static T GetValue<T>(this IMetaObject o, string PropertyName)
        {
            return GetValue<T>(o.GetProperty(PropertyName));
        }

        /// <summary>
        /// Gets the value of the object casted to the generic type
        /// </summary>
        /// <typeparam name="T">The type to cast the object as</typeparam>
        /// <param name="o">The source</param>
        /// <returns>The casted value</returns>
        public static T GetValue<T>(this IMetaObject o)
        {
            return (T)o.GetValue(typeof(T));
        }

        /// <summary>
        /// Gets the value of the object casted to the type variable
        /// </summary>
        /// <param name="o">The source</param>
        /// <param name="t">The type to cast the object as</param>
        /// <returns>The casted value</returns>
        public static object GetValue(this IMetaObject o, System.Type t)
        {
            return o.Value.Convert(t);
        }

        /// <summary>
        /// Checks if a property with the given name exists
        /// </summary>
        /// <param name="o">The source</param>
        /// <param name="PropertyName">The property name</param>
        /// <returns>A bool indicating whether or not the property exists</returns>
        public static bool HasProperty(this IMetaObject o, string PropertyName)
        {
            return o.Properties.Any(p => p.Property.Name == PropertyName);
        }

        /// <summary>
        /// Checks if a property of a matching IMetaProperty exists
        /// </summary>
        /// <param name="o">The source</param>
        /// <param name="property">The IMetaProperty to match against</param>
        /// <returns>A bool indicating whether or not the property exists</returns>
        public static bool HasProperty(this IMetaObject o, IMetaProperty property)
        {
            return o.Properties.Any(p => p.Property.Name == property.Name);
        }

        /// <summary>
        /// Gets an IMetaAttribute based on the name (apparently)
        /// </summary>
        /// <param name="o">the source object</param>
        /// <param name="IMetaAttributeName">The attribute name</param>
        /// <returns>The attribute</returns>
        public static IMetaAttribute IMetaAttribute(this IMetaObject o, string IMetaAttributeName)
        {
            return o.Property?.Attributes?.FirstOrDefault(a => a.Type.Name == IMetaAttributeName) ??
                   o.Type.Attributes.FirstOrDefault(a => a.Type.Name == IMetaAttributeName);
        }

        /// <summary>
        /// Casts and IMetaObject as a dictionary of the given type by casting each parameter
        /// </summary>
        /// <typeparam name="X">The Key type</typeparam>
        /// <typeparam name="Y">The Value type</typeparam>
        /// <param name="o">The source object</param>
        /// <returns>A dictionary containing the value of the IMetaObject</returns>
        public static Dictionary<X, Y> ToDictionary<X, Y>(this IMetaObject o)
        {
            Dictionary<X, Y> toReturn = new Dictionary<X, Y>();

            foreach (MetaObject thisObject in o.CollectionItems)
            {
                X key = thisObject["Key"].GetValue<X>();
                Y value = thisObject["Value"].GetValue<Y>();

                toReturn.Add(key, value);
            }

            return toReturn;
        }

        /// <summary>
        /// Attempts to convert the MetaObject into Json
        /// </summary>
        /// <param name="o">The source object</param>
        /// <returns>A Json string representation of the object</returns>
        public static string ToJson(this IMetaObject o)
        {
            return o.ToJson(new StringBuilder()).ToString();
        }

        internal static StringBuilder ToJson(this IMetaObject o, StringBuilder b)
        {
            CoreType thisCoreType = o.GetCoreType();

            switch (thisCoreType)
            {
                case CoreType.Value:
                    b.Append("\"" + o.Value + "\"");
                    break;

                case CoreType.Reference:
                    b.Append(" { ");
                    for (int i = 0; i < o.Properties.Count; i++)
                    {
                        IMetaObject m = o.Properties[i];

                        b.Append($"\"{m.Property.Name}\": ");
                        m.ToJson(b);
                        b.Append(",");
                    }

                    b.Append($"\"$ToString\": \"{o.Value}\"");

                    b.Append("}");
                    break;

                case CoreType.Collection:
                    b.Append(" [ ");
                    for (int i = 0; i < o.CollectionItems.Count; i++)
                    {
                        IMetaObject m = o.CollectionItems[i];
                        m.ToJson(b);
                        if (i != o.CollectionItems.Count - 1)
                        {
                            b.Append(",");
                        }
                    }
                    b.Append(" ] ");
                    break;
            }

            return b;
        }

        #endregion Methods
    }
}