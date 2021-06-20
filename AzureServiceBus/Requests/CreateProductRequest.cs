using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.API.Requests
{
    public class CreateProductRequest
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

    }
}
