#   **Fire#** for Firebase

.NET wrapper for the Firebase backend API.

Changes are sent to all subscribed clients automatically, so you can
update your clients **in realtime from the backend**.

####Installation (NuGet)
```csharp
Install-Package FireSharp
```
### Usage
```csharp
IFirebaseConfig config = new FirebaseConfig { AuthSecret = "**your firebase auth secret**", 
                                              BasePath = "**your firebase path**" };
IFirebaseClient  client = new FirebaseClient(config);
```
So far, supported methods are :
```csharp
client.Set(path, data)
client.Get(path)
client.Push(path, data)
client.Delete(path)
client.Update(path,data)
    
client.SetTaskAsync(path, data)
client.GetTaskAsync(path)
client.PushTaskAsync(path, data)
client.DeleteTaskAsync(path)
client.UpdateTaskAsync(path,data)
```
####Set
```csharp

var todo = new Todo {
                name = "Execute SET",
                priority = 2
            };
SetResponse response = _client.Set("todos/set", todo);
Todo result = response.ResultAs<Todo>(); //The response will contain the data written
```
####Push
```csharp

 var todo = new Todo {
                name = "Execute PUSH",
                priority = 2
            };
PushResponse response = _client.Push("todos/push", todo);
response.Result.Name //The result will contain the child name of the new data that was added
```
####Get
```csharp

 FirebaseResponse response = _client.Get("todos/set");
 Todo todo=response.ResultAs<Todo>(); //The response will contain the data being retreived
```
####Update
```csharp
var todo = new Todo {
                name = "Execute UPDATE!",
                priority = 1
            };

FirebaseResponse response = _client.Update("todos/set", todo);
Todo todo = response.ResultAs<Todo>(); //The response will contain the data written
```
####Delete
```csharp

DeleteResponse response = _client.Delete("todos"); //Deletes todos collection
response.Success; //Delete success flag
```
####PushTaskAsync
```csharp
var todo = new Todo {
                name = "Do your homework!",
                priority = 1
            };

PushResponse response = await _client.PushTaskAsync("todos", todo);
response.Result.Name //The result will contain the child name of the new data that was added
```
More features and documentation are coming soon.

More information about Firebase and the Firebase API is available at the
[official website](http://www.firebase.com/).

### Copyright

Copyright (c) 2013 Ziya SARIKAYA (@ziyasal)

