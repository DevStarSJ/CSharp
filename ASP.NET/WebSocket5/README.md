#WebSockets in .NET Core

`.NET Core` (현재는 ASP.NET 5)로 간단한 Web Socket을 구현하는 예제코드와 설명입니다.

작업은 `Visual Studio 2015` , `ASP.NET5-rc1` 으로 진행하였습니다.

##개요

- `.NET Core` (ASP.NET 5 Template)로 Web Application Project를 생성
- Server : Echo Server 를 Web Socket으로 구현
  - Client에서 전달한 Text Message를 그대로 전달
- Client : jQuery를 이용한 간단한 Web Socket page를 생성
- 여러 Client가 접속한 경우 접속된 모든 Client에게 echo message를 전달

##1. Project 생성

`C#` -> `Web` -> `ASP.NET Web Application` 선택 후 `ASP.NET 5 Templates` -> `Web Appliacaion`으로 Project를 생성합니다. 
`ASP.NET 5 Templates`가 설치되지 않은 경우 아래 설치 버튼이 활성화되며, 설치가 짧은 순간에 끝나지는 않으니 조금 기다리셔야 합니다. 

![그림 WS5.01](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.01.png?raw=true)  
![그림 WS5.02](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.02.png?raw=true)  

##2. Server Web Socket 구현

`Startup.cs` 파일을 열어서 `public void Configure()` 함수에 아래 Code를 추가합니다. 
단 `app.UseMvc()`보다 위에 위치해야 합니다. 

```C#
app.UseWebSockets();
app.Use(async (http, next) =>
{
    if (http.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await http.WebSockets.AcceptWebSocketAsync();

        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            // Handle the socket here
            while (webSocket.State == WebSocketState.Open)
            {
                var token = CancellationToken.None;
                var buffer = new ArraySegment<byte>(new byte[4096]);

                var received = await webSocket.ReceiveAsync(buffer, token);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);

                        // Handle request here
                        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, token);
                        break;
                }
            }
        }
    }
    else
    {
        await next();
    }
});
```

위 Code가 실행되려면 Assembly를 하나 추가해 줘야합니다.

>"Microsoft.AspNet.WebSockets.Server": "1.0.0-rc1-final"

`Nuget package manager`를 사용하지 않더라도 오류가 발생한 Code에서 `Ctrl + .` (Quick Action)을 이용하면 자동으로 추가가 됩니다.

![그림 WS5.03](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.03.png?raw=true)  

또 필요한 `namespace`를 `using`에 추가를 해주는 작업도 `Ctrl +.`으로 쉽게 작업이 가능합니다.

![그림 WS5.03](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.04.png?raw=true)  

Client에서 Connection을 연결할때마다 `app.Use`안에 선언한 함수가 실행됩니다. 
Request가 아닌 경우에는 그냥 무시 (`next();`)를 하며 `Request`인 경우에는 `Accept`수행 후 `Receive`작업을 기다리며 Pending 상태가 됩니다.

```C#
var webSocket = await http.WebSockets.AcceptWebSocketAsync();

if (webSocket != null && webSocket.State == WebSocketState.Open)
{
    while (webSocket.State == WebSocketState.Open)
    {
        var token = CancellationToken.None;
        var buffer = new ArraySegment<byte>(new byte[4096]);

        var received = await webSocket.ReceiveAsync(buffer, token);

        // Processing received message
    }
}
```

Message가 도착하면 `await webSocket.ReceiveAsync()`의 Pending이 해제되면서 다음 Line을 수행합니다.
현재 예제는 Text 타입에 대해서만 그대로 Client에게 echo message를 전달하고 있습니다.

```C#
switch (received.MessageType)
{
    case WebSocketMessageType.Text:
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, token);
        break;
}
```

##3. Client Web Socket 구현

편의상 추가로 `Controller`를 추가하지 않고 `Home` Controller에 `Chat`이라는 `Action Method`를 추가하도록 하겠습니다. 
`Controllers/HomeController.cs`파일을 열어서 아래 Action을 추가해 주세요.

```C#
public ActionResult Chat()
{
    return View();
}
```

아무런 내용없이 그냥 기본 Routing되는 View를 return합니다.

`Views/Home` 폴더 아래에 `Chat.cshtml`파일을 추가해서 다음과 같이 입력해주세요.

```HTML
@{
    ViewData["Title"] = "WebSocket Chat Page";
}

<form id="chatform" action="">
    <input id="inputbox" />
</form>
<div id="message" />

<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
<script type="text/javascript">
    $(document).ready(function()
    {
        var username = prompt('Enter your name: ');
        var uri = 'ws://localhost:9258';
        var ws = new WebSocket(uri);

        ws.onopen = function () {
            $('#message').prepend('<div>Connected.</div>');
            $('#chatform').submit(function (event) {
                ws.send('<strong>' + username + ' : </strong>' +$('#inputbox').val());
                $('#inputbox').val('');
                event.preventDefault();
             });
        };

        ws.onerror = function (event) {
            $('#message').prepend('<div>ERROR</div>');
        };

        ws.onmessage = function (event) {
            $('#message').prepend('<div>' + event.data + '</div>');
        };
    });
</script>
```

- 접속주소(`uri`) 값은 Server 실행시 port 번호를 보고 수정해 주세요.

`jQuery`를 nuget으로 설치해도 되지만 편의상 online으로 참조하였습니다.

- 해당 View가 열릴때 `WebSocket()`으로 접속합니다.
- 접속에 성공하면 (`ws.onopen`) `chatform`의 `submit`작업으로 `ws.send()`를 실행하도록 설정합니다.
- `WebSocket`으로 부터 message를 전달받으면 (`ws,onmessage`)를 화면에 출력합니다.

최대한 군더더기 없이 간단하게 구현하였습니다.

##4. 실행

`F5`를 눌러서 `Debug` 수행후 접속주소에 `/Home/Chat`를 붙여서 우리가 생성한 View를 열어주세요.

![그림 WS5.06](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.06.png?raw=true)  

이름을 입력해서 접속을 한 후에, 채팅 메세지를 입력하면 화면에 메세지가 출력됩니다. 

![그림 WS5.07](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.07.png?raw=true)  

위 메세지는 Client에서 출력을 한 것이 아니라 Server로 부터 전달받은 메세지 입니다. 
믿기 힘드시겠다면, 여러 Client끼리 Chat을 할 수 있도록 Server를 수정해 보도록 하겠습니다.

##5. Server를 여러 Client에게 메세지 전달하도록 수정

`Startup.cs` 파일의 `public class Startup`에 thread-safety한 map을 하나 선언합니다.

```C#
ConcurrentBag<WebSocket> _sockets = new ConcurrentBag<WebSocket>();
```

![그림 WS5.05](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.05.png?raw=true)  

다음으로는 Client에서 접속시 `_sockets`에 socket들을 저장해 놓습니다. 
`// Handle the socket here` 바로 윗 부분에 아래 Code를 추가합니다.

```C#
_sockets.Add(webSocket);
```

Client에게 `SendAsync()`를 하는 부분을 전체 Client에게 전송하도록 수정합니다.
`// Handle request here` 아래에 있는 `await webSocket.SendAsync()`줄을 지우고 아래 Code를 입력해주세요.

```C#
foreach (var socket in _sockets)
{
    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, token);
}
```

##6. 실행

이제 실행하여 Browser를 2개 띄워서 Test해보면 한 쪽에서 입력해도 양쪽으로 모두 메세지가 출력되는 것을 확인 할 수 있습니다.

![그림 WS5.08](https://github.com/DevStarSJ/Study/blob/master/Blog/MVC/WebSocket.5/image/ws5.08.png?raw=true)  

##참조 Site

- ASP.NET 5 Server Web Socket

<https://medium.com/@turowicz/websockets-in-asp-net-5-6094319a15a2#.rpft766wb>

- Client jQuery Web Socket

<https://blogs.msdn.microsoft.com/youssefm/2012/07/17/building-real-time-web-apps-with-asp-net-webapi-and-websockets>