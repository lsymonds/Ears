using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ears
{
    /// <summary>
    /// Contains a number of extensions relating to assemblies.
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the dispatchers and listeners Ears can use from a collection of source assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to search through to find events and listeners.</param>
        /// <returns>An enumerable collection containing a tuple of a listener and the event it listens to.</returns>
        internal static IEnumerable<(Type listener, Type ofEvent)> GetDispatchersAndListeners(
            this IEnumerable<Assembly> assemblies)
        {
            bool FilterInterfaces(Type type) =>
                type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEventListener<>);

            foreach (var assembly in assemblies)
            {
                var listeners = assembly.GetTypes()
                    .Where(x => !x.IsAbstract && !x.IsInterface)
                    .Where(x => x.GetInterfaces().Any(FilterInterfaces));

                foreach (var listener in listeners)
                {
                    var ofEventTypes = listener.GetInterfaces()
                        .Where(FilterInterfaces)
                        .SelectMany(i => i.GenericTypeArguments);

                    foreach (var e in ofEventTypes)
                        yield return (listener, e);
                }
            }
        }
    }
}
