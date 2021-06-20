using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.API.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ServiceBusClient _serviceBusClient;

        private readonly ServiceBusSender _serviceBusSender;

        public MessagePublisher(ServiceBusClient serviceBusClient)
        {

        }

        public Task Publish<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public Task Publish(string raw)
        {
            throw new NotImplementedException();
        }
    }
}
