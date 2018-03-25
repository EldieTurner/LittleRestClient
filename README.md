# LittleRestClient 
[![Build Status](https://travis-ci.org/EldieTurner/LittleRestClient.svg?branch=master)](https://travis-ci.org/EldieTurner/LittleRestClient)

Working on an easy rest client compatible with .net core and standard 2.0.  It isn't designed to be a verbose solution.  If you just need to do basic calls to an api this should work great.

## Installation

Available on [Nuget](https://www.nuget.org/packages/LittleRestClient/)

**Visual Studio:**

```PM> Install-Package LittleRestClient```

**.NET Core CLI:**

```dotnet add package LittleRestClient```

## Constructors

The simplest way to use LittleRestClient is pass the api's base url into the constructor.
```csharp
var restClient = new RestClient("http://example.com/api/");
```

There is also a constructor that takes an `IRestClientConfig` object.  You can implement this interface, and pass it into the constructor.
```csharp
    public interface IRestClientConfig
    {
        string BaseUrl { get; } //Required eg "Https://example.com/api/"
        string ApiToken { get; } //Blank by default
        string UserAgent { get; } //"LittleRestClient"
        string ContentType { get; } //"application/json"
        string AcceptType { get; } //"application/json"
    }
```

The only proeprty that is required is `BaseUrl`

## Public Methods
All public methods are `Async` so they are all wrapped in a `Task`.  Also all public methods other than `GetStringAsync` return a `RestClientResponse` or `RestClientResponse<TResult>` object depending on the response expected. These objects have the following properties 
```csharp
    public class RestClientResponse
    {
        public virtual HttpResponseHeaders Headers { get; internal set; }
        public virtual bool IsSuccessStatusCode { get; internal set; }
        public virtual HttpStatusCode StatusCode { get; internal set; }
        public virtual string ReasonPhrase { get; internal set; }
    }
        //Or if you are expecting data back.
    public class RestClientResponse<TResult> : RestClientResponse
    {
        public virtual TResult Data { get; internal set; }
    }
```

### GetStringAsync

This method will do a `GET` to the endpoint descripbed in the `route`. It will return the body of the response as a string.  by default this response will be in json format.  I usually use this call to test endpoints, and view the json so I can create objects to deserialize into. It can be used like below.
```csharp
string route = $"users/{userId}";
string json = await restClient.GetStringAsync(route);
```
Notice that the route is just what you need to add to the base url to complete the full path.

### GetAsync< TResult>

This method will do a `GET` to the endpoint descripbed in the `route`. It takes a generic that describes what kind of object the method should deserialize the data into.

```csharp
string route = $"users/{userId}";
RestClientResponse<User> response = await restClient.GetAsync<User>(route);
User user = response.Data;
```
### PostAsync< TBody>

This method will do a `POST` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response doesn't contain a payload only the call status. 

```csharp
string route = $"users";
User user = new User();
RestClientResponse response = await restClient.PostAsync(route, user);
return response.IsSuccessStatusCode;
```

### PostAsync<TBody,TResult>

This method will do a `POST` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response contain a payload that will be deserialized into a `TResult` object in the `data` field of the `RestClientResponse`. 

```csharp
string route = $"users";
User user = new User();
RestClientResponse<UserInfo> response = await restClient.PostAsync<User, UserInfo>(route, user);
UserInfo userInfo = response.Data;
```

### PutAsync< TBody>
  
This method will do a `PUT` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response doesn't contain a payload only the call status. 

```csharp
string route = $"users/{userId}";
RestClientResponse response = await restClient.PutAsync(route, user);
return response.IsSuccessStatusCode;
```
  
### PutAsync<TBody,TResult>

TThis method will do a `PUT` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response contain a payload that will be deserialized into a `TResult` object in the `data` field of the `RestClientResponse`. 

```csharp
string route = $"users/{userId}";
RestClientResponse<UserInfo> response = await restClient.PutAsync<User, UserInfo>(route, user);
UserInfo userInfo = response.Data;
```

### PatchAsync< TBody>

This method will do a `PATCH` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response doesn't contain a payload only the call status. 

```csharp
string route = $"users/{userId}";
RestClientResponse response = await restClient.PatchAsync(route, user);
return response.IsSuccessStatusCode;
```

### PatchAsync<TBody,TResult>

TThis method will do a `PATCH` to the endpoint descripbed in the `route`. It takes an object `body` that it will serialize and send. The response contain a payload that will be deserialized into a `TResult` object in the `data` field of the `RestClientResponse`. 

```csharp
string route = $"users/{userId}";
RestClientResponse<UserInfo> response = await restClient.PatchAsync<User, UserInfo>(route, user);
UserInfo userInfo = response.Data;
```

### DeleteAsync

This method will do a `DELETE` to the endpoint descripbed in the `route`. It doesn't send or receive a payload.  The response only contains the call status. 

```csharp
string route = $"users/{userId}";
RestClientResponse response = await restClient.DeleteAsync(route);
return response.IsSuccessStatusCode;
```

### DeleteAsync< TResult>

TThis method will do a `DELETE` to the endpoint descripbed in the `route`. It doesn't send a payload. The response contain a payload that will be deserialized into a `TResult` object in the `data` field of the `RestClientResponse`. 

```csharp
string route = $"users/{userId}";
RestClientResponse<UserInfo> response = await restClient.DeleteAsync<UserInfo>(route);
UserInfo userInfo = response.Data;
```

### Dispose

`RestClient` implements `IDisposable` (because HttpClient implements it).  You can override Dispose if you need to. (by default it just calls the dispose method on `HttpClient`)

## Protected Methods

If you need more or different functionallity than what you see above, you can always create a class that inherits from `RestClient`


### SerializeObject< TBody>

By default all serialization and deserialization is done with Newtonsoft.Json.  If you need xml serialization, or something else, you can overwrite this function, and plug in your own **String** serializer.

### CreateRequestContent< TBody>

Notice in the SerializeObject description it enphasizes string serializer.  If you need to send content as somethign other than string.  `byte[]` for examle, you need to create a different `HttpContent` type.  THis function is where that can happen.

### DeserializeObjectAsync< TResult>

Deserialize is also Newtonsoft.Json by default.  you can overwrite this to deserializae with a different deserializer, unlike the serialize method it could be of any `HttpContent` type.
