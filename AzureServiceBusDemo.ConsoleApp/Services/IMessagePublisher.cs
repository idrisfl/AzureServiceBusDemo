using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp.Services
{
    public interface IMessagePublisher
    {

        public Task Publish<T>(T obj);

        public Task PublishMessages<T>(IEnumerable<T> items);

        public Task Publish(string raw);
    }
}
