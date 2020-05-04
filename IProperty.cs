using Penguin.Reflection.Extensions;
using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Objects;
using System;
using System.Collections.Generic;

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
        public static T GetValue<T>(this IMetaProperty p, IMetaObject target)
        {
            if (p is null)
            {
                throw new System.ArgumentNullException(nameof(p));
            }

            if (target is null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            return target.GetValue<T>(p.Name);
        }

        /// <summary>
        /// Gets the value of the property from the speficied source as a string
        /// </summary>
        /// <param name="p">The IMetaProperty defining the value to be returned</param>
        /// <param name="target">The source object</param>
        /// <returns>The value of the property from the speficied source as a string</returns>
        public static string GetValue(this IMetaProperty p, IMetaObject target)
        {
            if (p is null)
            {
                throw new System.ArgumentNullException(nameof(p));
            }

            if (target is null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            return target[p.Name].Value;
        }

        public static bool TestFlags(long val, long flags)
        {
            if (val == 0)
            {
                return flags == 0;
            }

            if (flags == 0)
            {
                return val == 0;
            }

            for (int i = 0; i < 64; i++)
            {
                if (((flags >> i) & 0x1) != 0)
                {
                    if (((val >> i) & 0x1) == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets flags set on an enum property
        /// </summary>
        /// <param name="p">The propertyInfo</param>
        /// <param name="target">The instance of the object</param>
        /// <param name="otherFlags">returns a long representing the value of all flags on the object value that aren't declared explicitely</param>
        /// <returns>A list of the set enum values</returns>
        public static IList<EnumValue> GetFlags(this IMetaProperty p, IMetaObject target, out long otherFlags)
        {
            otherFlags = p.GetValue(target).Convert<long>();

            List<EnumValue> toReturn = new List<EnumValue>();

            foreach (EnumValue thisValue in p.GetFlags(target))
            {
                toReturn.Add(thisValue);

                otherFlags &= ~thisValue.Value.Convert<long>();
            }

            return toReturn;
        }

        /// <summary>
        /// Gets flags set on an enum property
        /// </summary>
        /// <param name="p">The propertyInfo</param>
        /// <param name="target">The instance of the object</param>
        /// <returns>A list of the set enum values</returns>
        public static IEnumerable<EnumValue> GetFlags(this IMetaProperty p, IMetaObject target)
        {
            if (p is null)
            {
                throw new System.ArgumentNullException(nameof(p));
            }

            if (target is null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            if (!p.Type.HasAttribute<FlagsAttribute>())
            {
                throw new ArgumentException($"Property type {p.Type.FullName} does not have flags attribute");
            }

            long l = p.GetValue(target).Convert<long>();

            foreach (EnumValue thisValue in p.Type.Values)
            {
                long thisVal = thisValue.Value.Convert<long>();

                if (TestFlags(l, thisVal))
                {
                    yield return thisValue;
                }
            }
        }

        #endregion Methods
    }
}