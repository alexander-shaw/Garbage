# In-Class Memory Leak Exercise

## Overview
This folder contains an exercise to diagnose and fix a memory leak during class.

### Learning
- Event publishers hold strong references to subscribers through delegate chains.
- Subscribers cannot be garbage collected while the publisher references them.
- Implementing IDisposable and unsubscribing in Dispose() breaks the reference chain.
- Always dispose event subscribers when they are no longer needed.

## Pattern
The exercise demonstrates an **event handler leak** where:
- A long-lived `EventPublisher` has an event.
- Many `EventSubscriber` instances subscribe to the event.
- Subscribers are never unsubscribed, so they cannot be garbage collected.
- The solution: implement `IDisposable` and unsubscribe in `Dispose()`.

## Demo Connection
- This exercise directly relates to: **Event Leak**
- All demos show the importance of proper resource management and disposal.