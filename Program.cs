using System.Net.WebSockets;
using System.Text;
using System.Net;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseWebSockets();
//Mudei de :
//app.MapGet("/", () => "Hello World!");

//app.Run();
//Para:

app.Map("/",async context =>
{
    //Primeiro vamos verificar se é uma requisição do tipo WebSocket
    if(!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    else{
        //using vai garantir o dispose automatico
        //Criamos um instancia do websocket aceitando a requisição
        using var websocket = 
            await context.WebSockets.AcceptWebSocketAsync();
        
        //Gera uma mensagem e enviar
        //Precisa ser um array de bytes
        //Vamos interpolar um string com a hora atual
        while(true) // vai ficar em um loop infinito
        {
            var data = Encoding.ASCII.GetBytes($".NET Rocks -> {DateTime.Now}");
            //Metodo que vamos usar para enviar uma mensagem de forma assincrona            
            await websocket.SendAsync(
                data,//Message
                WebSocketMessageType.Text, //Type json é um tipo texto também
                true,//EndOfMessage
                CancellationToken.None
            );//task

            await Task.Delay(1000); //Vai esperar um segundo
        }
    }
});

await app.RunAsync();