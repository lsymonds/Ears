# Moogie.Events

Moogie.Events is a simple observer pattern implementation of events and listeners. It is designed to help you decouple
key parts of your application without ever getting in your way.

## Getting Started

Add the `Moogie.Events` package into your project:

`dotnet add package Moogie.Events`

Then add `Moogie.Events` to your application's service collection in your `Startup.cs` file:

```
services.AddMoogieEvents(new EventManagerOptions
{
    AssembliesToSearch = new[]
    {
        typeof(EventsAndListenersInAssemblyOne),
        typeof(EventsAndListenersInAssemblyTwo)
    }
});
```

`Moogie.Events` uses your dependency injection container as a factory to instantiate your event listeners on demand.
Issue #8 is open for people to vote on and contribute to if there is a requirement to remove this dependency.

## Getting logs from Moogie.Events

`Moogie.Events` allows you to specify an `ILoggerFactory` within the `EventManagerOptions` class which will then
be used to create a logger instance which will log to whatever providers you have configured. 

The `ILoggerFactory` can also be mapped automatically when registering `Moogie.Events` with your service collection by 
passing `true` as the second parameter to the `AddMoogieEvents` method.

### Creating Events and Listeners

You can create events and listeners by implementing the `IDispatchable` and `IEventListener<? implements IDispatchable>`
interfaces respectively.

### Dispatching Events

You can dispatch events by injecting the `IEventManager` interface as a dependency into your class(es) and calling the 
`Dispatch` method on that dependency with the events you wish to dispatch.

```csharp
public class AccountService
{
    private readonly IEventManager _eventManager;
    private decimal _balance = 500;
       
    public AccountService(IEventManager eventManager) => _eventManager = eventManager;
    
    public async Task MakeWithdrawal(decimal amount, CancellationToken token)
    {
        _balance -= amount;
        await _eventManager.Dispatch(new WithdrawalMade
        {
            Amount = amount
        }, token);
    }
}
```
