https://www.firebase.com/docs/rest-api.html

Streaming from the REST API
=============================

Firebase REST endpoints support the EventSource / Server-Sent Events protocol as well. To stream changes to a single location in your Firebase, you will need to do a few things:

Set the client's Accept header to "text/event-stream"
Respect HTTP Redirects, in particular HTTP status code 307
If the location requires permission to read, you must include the auth parameter (see above in Query Parameters)
In return, the server will send named events as the state of the data at the requested URL changes. The structure of these messages conforms to the EventSource protocol:

event: event name
data: JSON encoded data payload
The server may send the following events:

put
The JSON-encoded data will be an object with two keys: path and data
The path points to a location relative to the request URL
The client should replace all of the data at that location in its cache with the data given in the message

patch
The JSON-encoded data will be an object with two keys: path and data
The path points to a location relative to the request URL
For each key in the data, the client should replace the corresponding key in its cache with the data for that key in the message

keep-alive
The data for this event is null, no action is required

cancel
The data for this event is null
This event will be sent if the security rules cause a read at the requested location to no longer be allowed

auth_revoked
The data for this event is a string indicating that a the credential has expired
This event will be sent when the supplied auth parameter is no longer valid
Here's an example set of events that the server may send:

// Set your entire cache to {"a": 1, "b": 2}
event: put
data: {"path": "/", "data": {"a": 1, "b": 2}}


// Put the new data in your cache under the key 'c', so that the complete cache now looks like:
// {"a": 1, "b": 2, "c": {"foo": true, "bar": false}}
event: put
data: {"path": "/c", "data": {"foo": true, "bar": false}}


// For each key in the data, update (or add) the corresponding key in your cache at path /c,
// for a final cache of: {"a": 1, "b": 2, "c": {"foo": 3, "bar": false, "baz": 4}}
event: patch
data: {"path": "/c", "data": {"foo": 3, "baz": 4}}