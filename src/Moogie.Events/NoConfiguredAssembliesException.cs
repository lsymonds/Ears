using System;

namespace Moogie.Events
{
    /// <summary>
    /// Exception that is thrown when no assemblies are configured in the options.
    /// </summary>
    public class NoConfiguredAssembliesException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NoConfiguredAssembliesException"/> class.
        /// </summary>
        public NoConfiguredAssembliesException() : base("No assemblies were configured.")
        {
        }
    }
}
