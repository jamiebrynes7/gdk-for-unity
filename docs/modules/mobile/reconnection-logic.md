<%(TOC)%>

# Reconnection Logic for Mobile

## Why would my client disconnect?

### Unstable Connection / Cellular Reception

When creating a mobile application, you need to ensure that clients are able to play the game even on an unstable connection. If the connection is too unstable, this might mean that the client is not able to connect or stay connected, but you need to provide a connection flow that allows them to get properly disconnected and are able to reconnect as soon as the they have better reception.

### Pausing of applications

When pausing an application, there are two possible outcomes:

1. The game won’t send data anymore. If this is kept up for too long, SpatialOS will close the connection.
2. The OS on your device closes the application.

Option 2 is trivial as we can’t do much about it. If the application is completely closed, you will simply be brought back to the start screen the next time you open the app. From there you can connect and load the data necessary to continue the game.  


Option 1 requires a bit more thought. How do we want to react to a disconnect? Do we want to be able to reconnect to the current match? Should we allow an offline mode?

In the end this is a very game-specific question and it depends on what you want to offer to your users. We provide you with the simplest solution: If you disconnect, you will be brought back to the start screen from where you can reconnect to continue the game.

## How do we detect a disconnected client?

[Heartbeating](https://docs.improbable.io/unity/alpha/modules/player-lifecycle/heartbeating) is a technique used to ensure your client is still connected. There are two ways your client may be seen as disconnected due to failing too many heartbeats:

1. SpatialOS assumes you have disconnected
2. A server-worker assumes you have disconnected

The second heartbeat exists so that we can add additional clean-up logic whenever a client disconnects, e.g. deleting the player entity from the world.  

### SpatialOS believes you have disconnected

If SpatialOS (aka the Runtime) disconnects you, you will receive a DisconnectOp.This contains the reason behind being disconnected and will trigger a disconnect event.

You can use this event to listen to a disconnect and perform any kind of logic necessary.

### The server-worker believes you have disconnected

If SpatialOS believes you are still connected and your server-worker thinks you are disconnected, you won’t receive a DisconnectOp.

In most game designs, your client will have authority over exactly one entity: The player entity

If your server-worker disconnects you and you use our player lifecycle module, the server-worker will destroy your player entity. This will leave your client with no entities to be authoritative over and it is still connected. If your client is not authoritative over any entity, it will not be able to have any entities in its view and the world will appear to be empty.

You will have to implement logic to listen to a possible loss of the player entity and handle it by either

1. Request a new player entity
2. Disconnect & reconnect
