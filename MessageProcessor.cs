using System;
 
namespace My_MQTT_Server
{
    public static class MessageProcessor
    {
        /// <summary>
        /// Process the MQTT message. Any long running processes called should be awaitable
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<bool> ProcessMessage(string topic, string message)
        {
            if (topic == "securitySensors")
            {
                Console.WriteLine($"ProcessMessage received message with topic: '{topic}' and message: '{message}'.");
                return true;
            } else
            {
                Console.WriteLine($"ProcessMessage received message with topic: '{topic}' but is unable to handle this topic.");
                return false;
            }
        }
    }
}