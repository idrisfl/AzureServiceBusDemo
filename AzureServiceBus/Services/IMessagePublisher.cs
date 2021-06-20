using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.API.Services
{
    public interface IMessagePublisher
    {

        public Task Publish<T>(T obj);

        public Task Publish(string raw);
    }
}
