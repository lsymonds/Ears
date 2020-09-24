using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Ears
{
    /// <summary>
    /// Options used to configure an <see cref="EventManager"/> instance.
    /// </summary>
    public class EventManagerOptions
    {
        /// <summary>
        /// Gets or sets whether to auto-discover listeners.
        /// </summary>
        public bool AutoDiscoverListeners { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to parallelise listener .Handle() calls.
        /// </summary>
        public bool ParalleliseHandleCalls { get; set; } = true;

        /// <summary>
        /// Gets or sets the assemblies to search when adding listeners automatically but also when adding listeners
        /// to the IoC container.
        /// </summary>
        public List<Assembly> AssembliesToSearch { get; set; } = new List<Assembly> {Assembly.GetCallingAssembly()};
        
        /// <summary>
        /// Gets or sets the logger factory used to create a logger instance for the classes within this library.
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; }
    }
}
