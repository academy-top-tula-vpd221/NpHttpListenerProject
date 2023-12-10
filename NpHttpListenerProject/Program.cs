using System.Net;
using System.Text;

HttpListener server = new();

server.Prefixes.Add("http://127.0.0.1:5000/");

server.Start();

var context = await server.GetContextAsync();

var response = context.Response;
var request = context.Request;
var user = context.User;

var loacalEndPoint = request.LocalEndPoint;
var remoteEndPoint = request.RemoteEndPoint;
var uri = request.Url;
var headers = request.Headers;

var headersStr = "headers:<br>";
foreach (string key in headers.Keys)
    headersStr += $"{key}: {headers[key]}<br>";


string responseHtml = @$"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>    
    <title>Server Document</title>
</head>
<body>
    <h2>Hello world</h2>
    <h3>{loacalEndPoint}</h3>
    <h3>{remoteEndPoint}</h3>
    <h3>{uri}</h3>
    <h3>{headersStr}</h3>
</body>
</html>";

byte[] responseBytes = Encoding.UTF8.GetBytes(responseHtml);
response.ContentLength64 = responseBytes.Length;

using Stream outputStream = response.OutputStream;

await outputStream.WriteAsync(responseBytes);
await outputStream.FlushAsync();

Console.WriteLine("request handling");

server.Stop();

server.Close();