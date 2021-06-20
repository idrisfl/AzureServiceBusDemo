using AzureServiceBusDemo.Receiver.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusDemo.Receiver
{
    public class CustomerWorker : BackgroundService
    {
        private readonly ILogger<CustomerWorker> _logger;
        private readonly MessageReceiver messageReceiver;
        private string subscriptionName = "customersubscription";


        public CustomerWorker(ILogger<CustomerWorker> logger)
        {
            messageReceiver = new MessageReceiver();
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            base.StartAsync(cancellationToken);
            return Task.Run(() => messageReceiver.Start(subscriptionName));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            base.StopAsync(cancellationToken);
            return Task.Run(() => messageReceiver.Stop());
        }
    }
}
