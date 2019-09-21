﻿using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using Penguin.Reflection.Serialization.Constructors;
using Penguin.Reflection.Serialization.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class IEnumerableExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Converts an IEnumerable to an IEnumerable of MetObjects
        /// </summary>
        /// <typeparam name="T">The type of the list to convert</typeparam>
        /// <param name="source">The source IEnumerable</param>
        /// <param name="Hydrate">Should the return objects be hydrated?</param>
        /// <returns>And IEnumerable of converted objects</returns>
        public static IEnumerable<IMetaObject> ToMetaList<T>(this IEnumerable<T> source, bool Hydrate = false) => source.ToMetaList<T>(null, Hydrate);
        /// <summary>
        /// Converts an IEnumerable to an IEnumerable of MetObjects
        /// </summary>
        /// <typeparam name="T">The type of the list to convert</typeparam>
        /// <param name="source">The source IEnumerable</param>
        /// <param name="c">A constructor to use during the generation</param>
        /// <param name="Hydrate">Should the return objects be hydrated?</param>
        /// <returns>And IEnumerable of converted objects</returns>
        public static IEnumerable<IMetaObject> ToMetaList<T>(this IEnumerable<T> source, MetaConstructor c, bool Hydrate = false)
        {
            Contract.Requires(source != null);

            c = c ?? new MetaConstructor();

            foreach( T o in source)
            {
                IMetaObject m = new MetaObject(o, c);

                if(Hydrate)
                {
                    m.Hydrate();
                }

                yield return m;
            }
        }
    }
}
