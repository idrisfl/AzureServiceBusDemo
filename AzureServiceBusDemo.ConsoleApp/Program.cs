using Azure.Messaging.ServiceBus;
using AzureServiceBusDemo.ConsoleApp.Requests;
using AzureServiceBusDemo.ConsoleApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp
{
    class Program
    {

        static async Task Main()
        {

            while (true)
            {
                try
                {
                    MessagePublisher messagePublisher = new MessagePublisher();



                    // Publish in batches
                    var createOrderRequests = new List<CreateOrderRequest>();

                    for (int i = 1; i <= 5; i++)
                    {
                        createOrderRequests.Add(new CreateOrderRequest { Guid = Guid.NewGuid(), Name = $"Order{i}" });
                    }

                    await messagePublisher.PublishMessages(createOrderRequests);


                    var createCustomerRequest = new List<CreateCustomerRequest>();

                    for (int i = 1; i <= 5; i++)
                    {
                        createCustomerRequest.Add(new CreateCustomerRequest { Guid = Guid.NewGuid(), Name = $"Customer{i}" });
                    }

                    await messagePublisher.PublishMessages(createCustomerRequest);


                    // publish single messages
                    await messagePublisher.Publish(new CreateCustomerRequest { Guid = Guid.NewGuid(), Name = "SingleCustomer" });

                    Console.WriteLine("Next messages will be sent in few seconds...");
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }           
        }
    }
}
