//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();

using System.Diagnostics;
using MQTTnet.Samples.Server;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class Program
{
    private static void Main(string[] args)
    {
        Server_ASP_NET_Samples.Start_Server_With_WebSockets_Support().GetAwaiter().GetResult();
    }

    private string GetDebuggerDisplay() => ToString();
}