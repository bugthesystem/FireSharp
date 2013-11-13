#   **Fire#** for Firebase

.NET wrapper for the Firebase backend API.

Changes are sent to all subscribed clients automatically, so you can
update your clients **in realtime from the backend**.

### Usage
```csharp
<<<<<<< HEAD
IFirebaseClient  client = new FirebaseClient("your firebase url", "your firebase secret");
FirebaseResponse response = _client.Push("todos", new { name="Do your homework", priority=1 });
=======

    IFirebaseConfig config = new FirebaseConfig {
                AuthSecret = "**your firebase auth secret**", BasePath = "**your firebase path**"
            };
     IFirebaseClient  client = new FirebaseClient(config);
     FirebaseResponse response = _client.Push("todos", new {name="Do your homework",priority=1});
>>>>>>> Code refactoring, added more tests and updated readme.md
```

So far, supported methods are :

```csharp
<<<<<<< HEAD
IFirebaseClient  client = new FirebaseClient("your firebase url", "your firebase secret");
client.Set(path, data)
client.Get(path)
client.Push(path, data)
client.Delete(path)
client.Update(path,data)
=======

     IFirebaseConfig config = new FirebaseConfig {
                AuthSecret = "**your firebase auth secret**", BasePath = "**your firebase path**"
            };
    IFirebaseClient  client = new FirebaseClient(config);
    
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
>>>>>>> Code refactoring, added more tests and updated readme.md
```

More features and documentation are coming soon.

More information about Firebase and the Firebase API is available at the
[official website](http://www.firebase.com/).

### Copyright

Copyright (c) 2013 Ziya SARIKAYA (@ziyasal)

