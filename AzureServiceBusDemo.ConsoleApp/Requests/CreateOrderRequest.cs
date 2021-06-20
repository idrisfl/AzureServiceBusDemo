using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp.Requests
{
    public class CreateOrderRequest
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }
    }
}
