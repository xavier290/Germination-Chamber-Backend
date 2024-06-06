using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                        o.ListenAnyIP(5009);
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
            });

            services.AddMqttConnectionHandler();
            services.AddConnections();

            services.AddDbContext<MqttDbContext>(options =>
                options.UseSqlServer("Server=XAVI;Database=MqttDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"),
                ServiceLifetime.Scoped);

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<MqttDbContext>()
                    .AddDefaultTokenProviders();

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
                        ValidIssuer = "http://localhost:5009",
                        ValidAudience = "http://localhost:5009",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9vL9PIK08MpvS5RD2AfuYatu8l/9WhANOULkmzdSL+E"))
                    };
                });

            services.AddScoped<MqttController>();
            services.AddScoped<MessageProcessor>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

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