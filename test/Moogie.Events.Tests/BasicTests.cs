using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Moogie.Events.Tests
{
    public class BasicTests
    {
        public class Event : IDispatchable
        {
            public string Message { get; set; }
        }

        public abstract class BaseListener : IEventListener<Event>
        {
            protected readonly Testor Testor;

            protected BaseListener(Testor testor) => Testor = testor;

            public Task Handle(Event dispatchedEvent)
            {
                Testor.Messages.Add(dispatchedEvent.Message);
                return Task.CompletedTask;
            }
        }

        public class Listener : BaseListener
        {
            public Listener(Testor testor) : base(testor)
            {}
        }

        public class SecondListener : BaseListener
        {
            public SecondListener(Testor testor) : base(testor)
            {}
        }

        public class Testor
        {
            public List<string> Messages { get; set; } = new List<string>();
        }

        [Fact]
        public async Task Event_Is_Listened_To()
        {
            // Arrange.
            var container = new ServiceCollection();
            container.AddSingleton(typeof(Testor), new Testor());
            container.AddMoogieEvents(new EventManagerOptions { AutoDiscoverListeners = false });

            var provider = container.BuildServiceProvider();
            var testor = provider.GetService<Testor>();
            var eventManager = provider.GetService<IEventManager>();
            eventManager.RegisterListeners(typeof(Event), typeof(Listener), typeof(SecondListener));

            // Act.
            await eventManager.Dispatch(new Event {Message = "Hello, Werld."});

            // Assert.
            Assert.Equal(2, testor.Messages.Count);
            Assert.Equal("Hello, Werld.", testor.Messages[0]);
            Assert.Equal("Hello, Werld.", testor.Messages[1]);
        }

        [Fact]
        public async Task Events_Are_Auto_Discovered()
        {
            // Arrange.
            var container = new ServiceCollection();
            container.AddSingleton(typeof(Testor), new Testor());
            container.AddMoogieEvents(new EventManagerOptions());

            var provider = container.BuildServiceProvider();
            var testor = provider.GetService<Testor>();
            var eventManager = provider.GetService<IEventManager>();

            // Act.
            await eventManager.Dispatch(new Event {Message = "Hello, Werld."});

            // Assert.
            Assert.Equal(2, testor.Messages.Count);
            Assert.Equal("Hello, Werld.", testor.Messages[0]);
            Assert.Equal("Hello, Werld.", testor.Messages[1]);
        }
    }
}
