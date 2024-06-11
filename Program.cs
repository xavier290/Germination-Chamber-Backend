//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();

using MQTTnet.Samples.Server;

// await Server_ASP_NET_Samples.Start_Server_With_WebSockets_Support();

internal class Program
{
    private static void Main(string[] args)
    {
        Server_ASP_NET_Samples.Start_Server_With_WebSockets_Support().GetAwaiter().GetResult();
    }
}