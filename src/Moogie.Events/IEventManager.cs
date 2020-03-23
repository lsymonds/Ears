using System;
using System.Threading.Tasks;

namespace Moogie.Events
{
    /// <summary>
    /// Defines the behavior that event managers must implement.
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// Dispatches a single dispatchable event.
        /// </summary>
        /// <param name="dispatchable">The dispatchable event.</param>
        /// <typeparam name="TDispatchable">
        /// The type of the dispatchable event. Must implement the <see cref="IDispatchable"/> interface.
        /// </typeparam>
        /// <returns>An awaitable task.</returns>
        Task Dispatch<TDispatchable>(TDispatchable dispatchable) where TDispatchable : IDispatchable
            => Dispatch(new[] {dispatchable});

        /// <summary>
        /// Dispatches multiple dispatchable events.
        /// </summary>
        /// <param name="dispatchables">The dispatchable events.</param>
        /// <typeparam name="TDispatchable">
        /// The type of the dispatchable events. Must implement the <see cref="IDispatchable"/> interface.
        /// </typeparam>
        /// <returns>An awaitable task.</returns>
        Task Dispatch<TDispatchable>(params TDispatchable[] dispatchables) where TDispatchable : IDispatchable;

        /// <summary>
        /// Registers a single listener for a dispatchable event.
        /// </summary>
        /// <param name="dispatchable">The type of the dispatchable event to register the listener against.</param>
        /// <param name="listener">The listener to register.</param>
        void RegisterListener(Type dispatchable, Type listener)
            => RegisterListeners(dispatchable, new[] {listener});

        /// <summary>
        /// Registers multiple listeners for a dispatchable event.
        /// </summary>
        /// <param name="dispatchable">The type of the dispatchable event to register the listener against.</param>
        /// <param name="listeners">The listener to register.</param>
        void RegisterListeners(Type dispatchable, params Type[] listeners);
    }
}
