using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SendSample
{
    class Program
    {
        private static QueueClient queueClient;
        private const string ServiceBusConnectionString = "Endpoint=sb://salesteamapp-pipe.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=63VLS1Jw+5mPpJF8s1JNgydA4UfbcEphlTg/hlgxT8k=";
        private const string QueueName = "salesmessages";

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);

            await SendMessagesToQueue(10);

            // Close the client after the ReceiveMessages method has exited. 
            await queueClient.CloseAsync();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        // Creates a Queue client and sends 10 messages to the queue. 
        private static async Task SendMessagesToQueue(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    // Create a new brokered message to send to the queue 
                    var message = new Message(Encoding.UTF8.GetBytes($"Message {i}"));

                    // Write the body of the message to the console 
                    Console.WriteLine($"Sending message: {Encoding.UTF8.GetString(message.Body)}");

                    // Send the message to the queue 
                    await queueClient.SendAsync(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                // Delay by 10 milliseconds so that the console can keep up 
                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
