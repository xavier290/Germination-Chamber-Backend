using System;
using ModelsMQTT_Server;


namespace My_MQTT_Server
{
    public class MessageProcessor
    {
        /// <summary>
        /// Process the MQTT message. Any long running processes called should be awaitable
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private readonly IServiceScopeFactory _scopeFactory;

        public MessageProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> ProcessMessage(string topic, string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<MqttDbContext>();

                try
                {
                    if (topic == "Temperature")
                    {
                        Console.WriteLine($"{topic}: {message} ºC.");

                        var measure = new Measurement
                        {
                            IdSensor = 1,
                            Topic = topic,
                            Message = message,
                            Fecha = DateTime.Now
                        };

                        _context.Measurements.Add(measure);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else if (topic == "Humidity")
                    {
                        Console.WriteLine($"{topic}: {message} ºC.");

                        var measure = new Measurement
                        {
                            IdSensor = 2,
                            Topic = topic,
                            Message = message,
                            Fecha = DateTime.Now
                        };

                        _context.Measurements.Add(measure);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else if (topic == "Luminosity")
                    {
                        Console.WriteLine($"{topic}: {message} ºC.");

                        var measure = new Measurement
                        {
                            IdSensor = 3,
                            Topic = topic,
                            Message = message,
                            Fecha = DateTime.Now
                        };

                        _context.Measurements.Add(measure);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else if (topic == "Start")
                    {
                        Console.WriteLine($"{topic}: '{message}'.");
                        // Process Start topic messages if necessary
                        return true;
                    }
                    else 
                    {
                        Console.WriteLine($"ProcessMessage received message with topic: '{topic}' but is unable to handle this topic.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing the message: {ex.Message}");
                    return false;
                }
            }
        }
    }
}