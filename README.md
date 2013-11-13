# firebase

.NET wrapper for the Firebase backend API.

Changes are sent to all subscribed clients automatically, so you can
update your clients **in realtime from the backend**.

### Usage
```csharp
IFirebaseClient  client = new FirebaseClient("your firebase url", "your firebase secret");
FirebaseResponse response = _client.Push("todos", new { name="Do your homework", priority=1 });
```

So far, supported methods are :

```csharp
IFirebaseClient  client = new FirebaseClient("your firebase url", "your firebase secret");
client.Set(path, data)
client.Get(path)
client.Push(path, data)
client.Delete(path)
client.Update(path,data)
```

More features are coming soon.

More information about Firebase and the Firebase API is available at the
[official website](http://www.firebase.com/).

### Copyright

Copyright (c) 2013 Ziya SARIKAYA (@ziyasal)

