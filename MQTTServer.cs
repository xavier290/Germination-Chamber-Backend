using MQTTnet.AspNetCore;
using MQTTnet.Server;
using My_MQTT_Server;
 
namespace MQTTnet.Samples.Server;
 
public static class Server_ASP_NET_Samples
{
    public static Task Start_Server_With_WebSockets_Support()
    {
        /*
         * This sample starts a minimal ASP.NET Webserver including a hosted MQTT server.
         */
        var host = Host.CreateDefaultBuilder(Array.Empty<string>())
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseKestrel(
                        o =>
                        {
                            // This will allow MQTT connections based on TCP port 1883.
                            o.ListenAnyIP(1883, l => l.UseMqtt());
 
                            // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
                            // See code below for URI configuration.
                            o.ListenAnyIP(5002); // Default HTTP pipeline
                        });
 
                    webBuilder.UseStartup<Startup>();
                });
 
        return host.RunConsoleAsync();
    }
 
    sealed class MqttController
    {
        public MqttController()
        {
            // Inject other services via constructor.
        }
 
        /// <summary>
        /// This event is triggered when the client publishes a message to the server
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public async Task<Task> OnClientPublishAsync(InterceptingPublishEventArgs eventArgs)
        {
            string topic = eventArgs.ApplicationMessage.Topic;
            string message = eventArgs.ApplicationMessage.ConvertPayloadToString();
 
            //Console.Write("Client payload:" + message);
 
            bool result = await MessageProcessor.ProcessMessage(topic, message);
 
            return Task.CompletedTask;
        }
 
        /// <summary>
        /// This event is called before OnClientPublishAsync and could be a good place to do security checks etc.
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
        {
            Console.WriteLine($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
            return Task.CompletedTask;
        }
 
    }
 
    sealed class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, MqttController mqttController)
        {
            app.UseRouting();
            app.UseCors("AllowAll");
 
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapConnectionHandler<MqttConnectionHandler>(
                        "/mqtt",
                        httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
                    endpoints.MapHealthChecks("/health");
                });
 
            app.UseMqttServer(
                server =>
                {
                    /*
                     * Attach event handlers etc. if required.
                     */
 
                    server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                    server.InterceptingPublishAsync += mqttController.OnClientPublishAsync;
 
                    //The following events might also be useful...
                    //server.ClientConnectedAsync += mqttController.OnClientConnected;
                    //server.InterceptingClientEnqueueAsync += mqttController.OnClientSendMessage;
                    //server.ClientSubscribedTopicAsync += mqttController.OnClientSubscribedTopic;
 
                });
        }
 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedMqttServer(
                optionsBuilder =>
                {
                    optionsBuilder.WithDefaultEndpoint();
                });
 
            services.AddMqttConnectionHandler();
            services.AddConnections();
 
            services.AddSingleton<MqttController>();
            services.AddHealthChecks();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}