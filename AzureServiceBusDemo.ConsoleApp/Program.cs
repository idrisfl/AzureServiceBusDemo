using Azure.Messaging.ServiceBus;
using AzureServiceBusDemo.ConsoleApp.Requests;
using AzureServiceBusDemo.ConsoleApp.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.ConsoleApp
{
    class Program
    {
        //// connection string to your Service Bus namespace
        //static string connectionString = "Endpoint=sb://myservicebus20210619.servicebus.windows.net/;SharedAccessKeyName=myservicebussas;SharedAccessKey=TYJTP9AnL6cOLDRSzKDDQu+T4o2NeaYzSejSkF2m24Q=;EntityPath=mymessages";

        //// name of your Service Bus topic
        //static string topicName = "mymessages";

        //// the client that owns the connection and can be used to create senders and receivers
        //static ServiceBusClient client;

        //// the sender used to publish messages to the topic
        //static ServiceBusSender sender;

        //// number of messages to be sent to the topic
        //private const int numOfMessages = 10;


        static async Task Main(string[] args)
        {
            MessagePublisher messagePublisher = null;

            try
            {
                //client = new ServiceBusClient(connectionString);
                //sender = client.CreateSender(topicName);

                messagePublisher = new MessagePublisher();

                object msg;
                // ServiceBusMessage sbm;
                //using (ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync())
                //{



                    for (int i = 1; i <= 10; i++)
                    {
                        if (i % 2 == 0)
                        {
                            msg = new CreateOrderRequest() { Guid = Guid.NewGuid(), Name = $"Order{i}" };
                            //sbm = new ServiceBusMessage(JsonConvert.SerializeObject(msg));
                        }
                        else
                        {
                            msg = new CreateCustomerRequest() { Guid = Guid.NewGuid(), Name = $"Customer{i}" };
                            //sbm = new ServiceBusMessage(JsonConvert.SerializeObject(msg));
                        }

                        await messagePublisher.Publish(msg);

                        //sbm.ApplicationProperties.Add("messageType", msg.GetType().Name);


                        ////// try adding a message to the batch
                        ////if (!messageBatch.TryAddMessage(sbm))
                        ////{
                        ////    // if it is too large for the batch
                        ////    throw new Exception($"The message {i} is too large to fit in the batch.");
                        ////}
                    //}

                    //try
                    //{
                    //    // Use the producer client to send the batch of messages to the Service Bus topic
                    //    await sender.SendMessagesAsync(messageBatch);
                    //    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");
                    //}
                    //finally
                    //{
                    //    // Calling DisposeAsync on client types is required to ensure that network
                    //    // resources and other unmanaged objects are properly cleaned up.
                    //    await sender.DisposeAsync();
                    //    await client.DisposeAsync();
                    //}
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                messagePublisher.CloseConnection();
            }




            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }
    }
}
