#   **Fire#** for Firebase

Firebase REST API wrapper for the .NET & Xamarin.

Changes are sent to all subscribed clients automatically, so you can
update your clients **in realtime from the backend**.

[![Circle CI](https://circleci.com/gh/ziyasal/FireSharp/tree/v2.svg?style=svg)](https://circleci.com/gh/ziyasal/FireSharp/tree/v2)

[![Stories in Ready](https://badge.waffle.io/ziyasal/firesharp.svg?label=ready&title=Ready)](http://waffle.io/ziyasal/firesharp)


**IMPORTANT :** [**v1 docs**](https://github.com/ziyasal/FireSharp/wiki/v1-Docs) moved [here](https://github.com/ziyasal/FireSharp/wiki/v1-Docs).

####Installation (NuGet)
```csharp
//**Install v2**
Install-Package FireSharp -Pre

//**Install v1**
Install-Package FireSharp
```
### Usage
[FirebaseClient](https://github.com/ziyasal/FireSharp/blob/master/FireSharp/FirebaseClient.cs) uses [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) by default.

#### How can I configure FireSharp?
------------------------------

```csharp
  IFirebaseConfig config = new FirebaseConfig
  {
     AuthSecret = "your_firebase_secret",
     BasePath = "https://yourfirebase.firebaseio.com/"
  };
````
```csharp
IFirebaseClient  client = new FirebaseClient(config);
```
So far, supported methods are :

####Set
```csharp

var todo = new Todo {
                name = "Execute SET",
                priority = 2
            };
SetResponse response = await _client.SetAsync("todos/set", todo);
Todo result = response.ResultAs<Todo>(); //The response will contain the data written
```
####Push
```csharp

 var todo = new Todo {
                name = "Execute PUSH",
                priority = 2
            };
PushResponse response =await  _client.PushAsync("todos/push", todo);
response.Result.Name //The result will contain the child name of the new data that was added
```
####Get
```csharp

 FirebaseResponse response = await _client.GetAsync("todos/set");
 Todo todo=response.ResultAs<Todo>(); //The response will contain the data being retreived
```
####Update
```csharp
var todo = new Todo {
                name = "Execute UPDATE!",
                priority = 1
            };

FirebaseResponse response =await  _client.UpdateAsync("todos/set", todo);
Todo todo = response.ResultAs<Todo>(); //The response will contain the data written
```
####Delete
```csharp

DeleteResponse response =await  _client.DeleteAsync("todos"); //Deletes todos collection
response.Success; //Delete success flag
```
####Listen **Streaming from the REST API**
```csharp
await _client.OnAsync("chat", (sender, args) => { 
       System.Console.WriteLine(args.Data); 
});
```

More information about Firebase and the Firebase API is available at the
[official website](http://www.firebase.com/).


## License
Code and documentation are available according to the *MIT* License (see [LICENSE](https://github.com/ziyasal/FireSharp/blob/master/LICENSE.md)).
