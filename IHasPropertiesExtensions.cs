using Penguin.Reflection.Serialization.Abstractions.Interfaces;
using System.Linq;

namespace Penguin.Reflection.Serialization.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class IHasPropertiesExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Retrieves a property from the selected type, recursively using "." as a delimiter
        /// </summary>
        /// <param name="target">The target to retrieve the property from</param>
        /// <param name="path">The path of the property to find</param>
        /// <returns>The object instance of the property being searched for</returns>
        public static IMetaProperty GetProperty(this IHasProperties target, string path)
        {
            if (target is null)
            {
                throw new System.ArgumentNullException(nameof(target));
            }

            if (path is null)
            {
                throw new System.ArgumentNullException(nameof(path));
            }

            IMetaProperty m = null;

            foreach (string chunk in path.Split('.'))
            {
                m = (m?.Type ?? target).Properties.FirstOrDefault(p => p.Name == chunk);

                if (m is null)
                {
                    return null;
                }
            }

            return m;
        }

        #endregion Methods
    }
}