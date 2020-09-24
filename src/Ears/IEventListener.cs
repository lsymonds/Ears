using System.Threading;
using System.Threading.Tasks;

namespace Ears
{
    /// <summary>
    /// Defines what all asynchronous event listeners must implement.
    /// </summary>
    /// <typeparam name="TDispatchable">The type implementing <see cref="IDispatchable"/> to listen for.</typeparam>
    public interface IEventListener<in TDispatchable> where TDispatchable : IDispatchable
    {
        /// <summary>
        /// Handles (processes) a dispatched <see cref="IDispatchable"/> implementation.
        /// </summary>
        /// <param name="dispatchedEvent">The type implementing <see cref="IDispatchable"/> that was dispatched.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        Task Handle(TDispatchable dispatchedEvent, CancellationToken token = default);
    }
}
