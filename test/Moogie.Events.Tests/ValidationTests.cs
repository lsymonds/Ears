using System;
using Xunit;

namespace Moogie.Events.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void Null_Options_Results_In_Exception()
        {
            // Arrange & Act.
            // ReSharper disable once ObjectCreationAsStatement
            static void Action() => new EventManager(null!, null!);

            // Assert.
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Fact]
        public void Providing_No_Assemblies_Results_In_An_Exception_Being_Thrown()
        {
            // Arrange & Act.
            // ReSharper disable once ObjectCreationAsStatement
            static void Action() => new EventManager(new EventManagerOptions { AssembliesToSearch = null }, null!);

            // Assert.
            Assert.Throws<NoConfiguredAssembliesException>(Action);
        }
    }
}
