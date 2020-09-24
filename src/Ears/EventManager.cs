using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[assembly:InternalsVisibleTo("Ears.Tests")]
namespace Ears
{
    /// <summary>
    /// Handles all event associated functionality.
    /// </summary>
    public class EventManager : IEventManager
    {
        private readonly EventManagerOptions _eventManagerOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventManager> _logger;

        private readonly ConcurrentDictionary<Type, List<Type>> _eventListeners =
            new ConcurrentDictionary<Type, List<Type>>();

        /// <summary>
        /// Initialises a new instance of the <see cref="EventManager"/> class with a set of options.
        /// </summary>
        /// <param name="eventManagerOptions">The options used to configure the event manager.</param>
        /// <param name="serviceProvider">Provider used to resolve dependencies.</param>
        internal EventManager(EventManagerOptions eventManagerOptions, IServiceProvider serviceProvider)
        {
            _eventManagerOptions = eventManagerOptions;
            _serviceProvider = serviceProvider;

            if (eventManagerOptions == null) throw new ArgumentNullException(nameof(eventManagerOptions));
            if (eventManagerOptions.AssembliesToSearch == null)
                throw new NoConfiguredAssembliesException();

            if (eventManagerOptions.AutoDiscoverListeners)
            {
                var listeners = eventManagerOptions.AssembliesToSearch.GetDispatchersAndListeners();
                foreach (var (listener, ofEvent) in listeners)
                    RegisterListeners(ofEvent, listener);
            }

            if (_eventManagerOptions.LoggerFactory != null)
                _logger = _eventManagerOptions.LoggerFactory.CreateLogger<EventManager>();
        }

        /// <inheritdoc />
        public void RegisterListener(Type dispatchable, Type listener) => RegisterListeners(dispatchable, listener);

        /// <inheritdoc />
        public void RegisterListeners(Type dispatchable, params Type[] listeners)
        {
            if (!_eventListeners.ContainsKey(dispatchable))
                _eventListeners[dispatchable] = new List<Type>();

            foreach (var listener in listeners.Except(_eventListeners[dispatchable]))
            {
                _logger?.LogInformation($"Adding new listener ({listener}) for event {dispatchable}.");
                _eventListeners[dispatchable].Add(listener);
            }
        }

        /// <inheritdoc />
        public Task Dispatch<TDispatchable>(TDispatchable dispatchable, CancellationToken token = default)
            where TDispatchable : IDispatchable => 
            Dispatch(token, dispatchable);

        /// <inheritdoc />
        public Task Dispatch<TDispatchable>(params TDispatchable[] dispatchables)
            where TDispatchable : IDispatchable =>
            Dispatch(CancellationToken.None, dispatchables);

        /// <inheritdoc />
        public async Task Dispatch<TDispatchable>(CancellationToken token, params TDispatchable[] dispatchables)
            where TDispatchable : IDispatchable
        {
            var handlers = new List<Task>();

            foreach (var dispatchable in dispatchables)
            {
                if (!_eventListeners.ContainsKey(dispatchable.GetType()))
                    continue;

                var appropriateListeners = _eventListeners[dispatchable.GetType()];
                foreach (var listener in appropriateListeners)
                {
                    _logger?.LogInformation("Dispatching event {dispatchable} to listener {listener}.");
                    
                    var listenerInstance = (IEventListener<TDispatchable>) _serviceProvider.GetService(listener);
                    handlers.Add(listenerInstance.Handle(dispatchable, token));
                }
            }

            if (_eventManagerOptions.ParalleliseHandleCalls)
                await Task.WhenAll(handlers).ConfigureAwait(false);
            else
                foreach (var handler in handlers)
                    await handler.ConfigureAwait(false);
        }
    }
}
