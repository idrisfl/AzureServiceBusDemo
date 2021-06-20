using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.Receiver.Services
{
    public class MessageReceiver
    {
        // connection string to your Service Bus namespace
        internal string connectionString = "<CONNECTION STRING>";

        // name of the Service Bus topic
        internal string topicName = "<TOPIC NAME>";

        // name of the subscription to the topic
        internal string subscriptionName;

        // the client that owns the connection and can be used to create senders and receivers
        internal ServiceBusClient client;

        // the processor that reads and processes messages from the subscription
        internal ServiceBusProcessor processor;


        public async void Start(string subscriptionName)
        {
            Console.WriteLine("...Starting");

            this.subscriptionName = subscriptionName;

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);

            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(topicName, this.subscriptionName, new ServiceBusProcessorOptions());

            // add handler to process messages
            processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start processing 
            await processor.StartProcessingAsync();

            //Console.ReadKey();
        }

        // handle received messages
        protected async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body} from subscription: {this.subscriptionName}");

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        protected Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async void Stop()
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            Console.WriteLine("...Stopping");
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
