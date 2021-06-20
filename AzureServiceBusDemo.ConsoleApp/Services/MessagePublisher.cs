using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp.Services
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        // connection string to your Service Bus namespace
        static string connectionString = "<CONNECTION STRING>";

        // name of your Service Bus topic
        static string topicName = "<TOPIC NAME>";

        private readonly ServiceBusClient _serviceBusClient;

        private ServiceBusSender _serviceBusSender;

        // number of messages to be sent to the topic
        private const int numOfMessages = 5;


        public MessagePublisher()
        {
            _serviceBusClient = new ServiceBusClient(connectionString);

        }

        public async Task Publish<T>(T obj)
        {
            try
            {
                OpenConnection();                
                var servicebusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(obj));
                servicebusMessage.ApplicationProperties.Add("messagetype", obj.GetType().Name);

                // Use the producer client to send the batch of messages to the Service Bus topic
                await _serviceBusSender.SendMessageAsync(servicebusMessage);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error publishing message: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task PublishMessages<T>(IEnumerable<T> items)
        {

            try
            {
                OpenConnection();

                using ServiceBusMessageBatch messageBatch = await _serviceBusSender.CreateMessageBatchAsync();
                foreach (var item in items)
                {
                    var servicebusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(item));
                    servicebusMessage.ApplicationProperties.Add("messagetype", item.GetType().Name);

                    // try adding a message to the batch
                    if (!messageBatch.TryAddMessage(servicebusMessage))
                    {
                        // if it is too large for the batch
                        throw new Exception($"The message {item} is too large to fit in the batch.");
                    }
                }

                await _serviceBusSender.SendMessagesAsync(messageBatch);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }     

            Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");
        }

        public Task Publish(string raw)
        {
            throw new NotImplementedException();
        }


        private void OpenConnection()
        {
            if (_serviceBusSender == null || _serviceBusSender.IsClosed)
            {
                _serviceBusSender = _serviceBusClient.CreateSender(topicName);
            }
        }

        private async void CloseConnection()
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            if (_serviceBusSender != null && !_serviceBusSender.IsClosed)
            {
                await _serviceBusSender.DisposeAsync();
            }
        }

        public async void Dispose()
        {
            if (_serviceBusClient != null && !_serviceBusClient.IsClosed)
            {
                await _serviceBusClient.DisposeAsync();
            }
        }
    }
}
