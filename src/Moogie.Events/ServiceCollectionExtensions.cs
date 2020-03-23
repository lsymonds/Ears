using System;
using Microsoft.Extensions.DependencyInjection;

namespace Moogie.Events
{
    /// <summary>
    /// Extension methods to add Moogie.Events to the standard dependency injection framework provided by Microsoft.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Moogie.Events to a specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection to add the project to.</param>
        /// <param name="eventManagerOptions">Options to configure the event manager.</param>
        /// <returns>The service collection with Moogie.Events added.</returns>
        public static IServiceCollection AddMoogieEvents(this IServiceCollection serviceCollection,
            EventManagerOptions eventManagerOptions)
        {
            EventManager BuildEventManager(IServiceProvider serviceProvider)
                => new EventManager(eventManagerOptions, serviceProvider);

            if (eventManagerOptions?.AssembliesToSearch != null)
            {
                var listeners = eventManagerOptions.AssembliesToSearch.GetDispatchersAndListeners();
                foreach (var (listener, _) in listeners)
                    serviceCollection.AddTransient(listener);
            }

            return serviceCollection
                .AddSingleton<IEventManager, EventManager>(BuildEventManager);
        }
    }
}
