<%(TOC)%>

# Reconnection logic

## Why a client would disconnect

### Unstable connection / cellular reception

When creating a mobile application, you need to ensure that clients are able to play the game even with an unstable connection.

If a client's connection is too unstable, they may not able to connect to your game or stay connected for long periods. You should provide a connection flow that allows them to disconnect properly, and handle reconnection as soon as they obtain better reception.

### Pausing of applications

When pausing an application, there are two possible outcomes:

1. The game stops sending data. If this happens for too long, SpatialOS will close the connection.
1. The device OS closes the application.

There is not much that can be done in the second case. If the application is completely closed, you will simply be brought back to the start screen the next time you open the app. From there you can connect and load the data necessary to continue the game.

However, the first case requires a bit more thought. How do we want to react to a disconnect? Do we want to be able to reconnect to the current match? Should we allow an offline mode?

In the end this is a very game-specific question that depends entirely on what you want to offer to your users. The FPS Starter Project implements the simplest solution: a disconnection takes you back to the start screen, from where you can reconnect to continue the game.

## How a disconnection is detected

[Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) is a technique used to verify that your client is still connected. If there are too many failed heartbeats, there are two ways your client may be seen as disconnected:

1. SpatialOS assumes you have disconnected.
1. A server-worker assumes you have disconnected.

The latter heartbeating logic exists so that we can add additional clean-up logic whenever a client disconnects, e.g. deleting the player entity from the world.

### SpatialOS believes you have disconnected

If SpatialOS (aka the Runtime) disconnects you, you will receive a `DisconnectOp`. This contains the reason behind the disconnection and will trigger a disconnect event.

You can listen for this event in order to perform any kind of disconnection logic necessary.

### The server-worker believes you have disconnected

If SpatialOS believes you are still connected and your server-worker thinks you are disconnected, you won’t receive a `DisconnectOp`.

In most game designs, your client will have authority over exactly one entity: the player entity. If your server-worker disconnects you and you use our player lifecycle module, the server-worker will destroy your player entity.

This will leave your client still connected to SpatialOS, but without any entities to be authoritative over. If your client is not authoritative over any entity, it will not be able to have any entities in its view and the client's world will appear to be empty.

You will have to implement logic to listen to a possible loss of the player entity and handle it by either:

1. Requesting a new player entity.
2. Disconnect & reconnect the client.
