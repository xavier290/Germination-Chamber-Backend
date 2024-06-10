﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using My_MQTT_Server;
using ModelsMQTT_Server;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace MQTTnet.Samples.Server
{
    public static class Server_ASP_NET_Samples
    {
        public static Task Start_Server_With_WebSockets_Support()
        {
            var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(o =>
                    {
                        o.ListenAnyIP(1883, l => l.UseMqtt());
                        o.ListenAnyIP(5002);
                        o.ListenAnyIP(8883, l => l.UseHttps()); // MQTT over WebSockets with SSL
                    });
                    webBuilder.UseStartup<Startup>();
                });

            return host.RunConsoleAsync();
        }
    }

    public class MqttController
    {
        private readonly MessageProcessor _messageProcessor;
        public MqttController(MessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        public async Task<Task> OnClientPublishAsync(InterceptingPublishEventArgs eventArgs)
        {
            string topic = eventArgs.ApplicationMessage.Topic;
            string message = eventArgs.ApplicationMessage.ConvertPayloadToString();

            bool result = await _messageProcessor.ProcessMessage(topic, message);

            if(!result) 
            {
                return Task.FromException(new Exception("An error occurred while processing the message."));
            }

            return Task.CompletedTask;
        }

        public Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
        {
            Console.WriteLine($"Client '{eventArgs.ClientId}' wants to connect. Accepting!");
            return Task.CompletedTask;
        }
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, MqttController mqttController)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseCors("CorsPolicy"); // Enable CORS
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                        protocolList => protocolList.FirstOrDefault() ?? string.Empty);
                endpoints.MapHealthChecks("/health");
            });

            app.UseMqttServer(server =>
            {
                server.ValidatingConnectionAsync += mqttController.ValidateConnection;
                server.InterceptingPublishAsync += mqttController.OnClientPublishAsync;
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedMqttServer(optionsBuilder =>
            {
                optionsBuilder.WithDefaultEndpoint();
                optionsBuilder.WithEncryptedEndpoint();
            });

            services.AddMqttConnectionHandler();
            services.AddConnections();

            services.AddDbContext<MqttDbContext>(options =>
                options.UseSqlServer("Server=tcp:germination-chamber.database.windows.net,1433;Initial Catalog=MqttDb;Persist Security Info=False;User ID=Xavi;Password=GerminationChamber172523@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"),
                ServiceLifetime.Scoped);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://dolphin-app-hlqw2.ondigitalocean.app/",
                    ValidAudience = "https://dolphin-app-hlqw2.ondigitalocean.app/",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9vL9PIK08MpvS5RD2AfuYatu8l/9WhANOULkmzdSL+E"))
                };
            });

            services.AddScoped<MqttController>();
            services.AddScoped<MessageProcessor>();
            services.AddScoped<UserRepository>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddHealthChecks();
            // Configure CORS to allow requests from your frontend
            // services.AddCors(options =>
            // {
            //     options.AddPolicy("CorsPolicy",
            //         builder => builder
            //         .WithOrigins("http://localhost:3000")
            //         .AllowAnyMethod()
            //         .AllowAnyHeader()
            //         .AllowCredentials());
            // });

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