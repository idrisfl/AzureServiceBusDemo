using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        // connection string to your Service Bus namespace
        static string connectionString = "Endpoint=sb://myservicebus20210619.servicebus.windows.net/;SharedAccessKeyName=myservicebussas;SharedAccessKey=TYJTP9AnL6cOLDRSzKDDQu+T4o2NeaYzSejSkF2m24Q=;EntityPath=mymessages";

        // name of your Service Bus topic
        static string topicName = "mymessages";

        private readonly ServiceBusClient _serviceBusClient;

        private ServiceBusSender _serviceBusSender;

        // number of messages to be sent to the topic
        private const int numOfMessages = 10;


        public MessagePublisher()
        {
            _serviceBusClient = new ServiceBusClient(connectionString);

        }

        public async Task Publish<T>(T obj)
        {
            try
            {
                OpenConnection();
                using ServiceBusMessageBatch messageBatch = await _serviceBusSender.CreateMessageBatchAsync();
                var sbm = new ServiceBusMessage(JsonConvert.SerializeObject(obj));
                sbm.ApplicationProperties.Add("messagetype", obj.GetType().Name);


                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(sbm))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {obj} is too large to fit in the batch.");
                }


                // Use the producer client to send the batch of messages to the Service Bus topic
                await _serviceBusSender.SendMessagesAsync(messageBatch);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error publishing message: {ex.Message}");
            }
        }

        public async Task Publish<T>(IEnumerable<T> items)
        {

            foreach (var item in items)
            {
                await this.Publish(item);
            }

            Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");

        }

        public Task Publish(string raw)
        {
            throw new NotImplementedException();
        }


        public void OpenConnection()
        {
            if (_serviceBusSender == null || _serviceBusSender.IsClosed)
            {
                _serviceBusSender = _serviceBusClient.CreateSender(topicName);
            }
        }

        public async void CloseConnection()
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            if (_serviceBusSender != null && !_serviceBusSender.IsClosed)
            {
                await _serviceBusSender.DisposeAsync();
            }
            if (_serviceBusClient != null && !_serviceBusClient.IsClosed)
            {
                await _serviceBusClient.DisposeAsync();
            }
        }
    }
}
