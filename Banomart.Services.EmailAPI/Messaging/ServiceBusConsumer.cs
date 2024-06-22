using Azure.Messaging.ServiceBus;
using Banomart.Services.EmailAPI.Models;
using Banomart.Services.EmailAPI.Service;
using Newtonsoft.Json;
using System.Text;

namespace Banomart.Services.EmailAPI.Messaging
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string newUserCreatedQueue;
        private readonly IConfiguration configuration;
        private readonly EmailService emailService;
        private ServiceBusProcessor emailCartProcessor;
        private ServiceBusProcessor newUserCreatedProcessor;

        public ServiceBusConsumer(IConfiguration config, EmailService emailService)
        {

            configuration = config;
            this.emailService = emailService;

            serviceBusConnectionString = configuration.GetValue<string>("AzureConfig:AzureBusConnectionString");

            emailCartQueue = configuration.GetValue<string>("AzureConfig:CartQueueName");
            newUserCreatedQueue = configuration.GetValue<string>("AzureConfig:NewuserQueueName");

            var client = new ServiceBusClient(serviceBusConnectionString);

            emailCartProcessor = client.CreateProcessor(emailCartQueue);
            newUserCreatedProcessor = client.CreateProcessor(newUserCreatedQueue);
        }

        public async Task Start()
        {
            emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            newUserCreatedProcessor.ProcessMessageAsync += OnNewUserCreatedRequestReceived;
            newUserCreatedProcessor.ProcessErrorAsync += ErrorHandler;

            await emailCartProcessor.StartProcessingAsync();
            await newUserCreatedProcessor.StartProcessingAsync();

        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var messageBody = JsonConvert.DeserializeObject<CartDto>(body);

            try 
            {
                await emailService.EmailCartLog(messageBody);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex) 
            {
                await args.AbandonMessageAsync(message);
            }
        }

        private async Task OnNewUserCreatedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var messageBody = JsonConvert.DeserializeObject<UserDto>(body);

            try
            {
                await emailService.EmailNewUser(messageBody);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                await args.AbandonMessageAsync(message);
            }
        }

        public async Task Stop()
        {
            await emailCartProcessor.StopProcessingAsync();
            await emailCartProcessor.DisposeAsync();
            await newUserCreatedProcessor.StopProcessingAsync();
            await newUserCreatedProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Log or handle the error here
            Console.WriteLine($"Error occurred: {args.Exception.Message}");
            return Task.CompletedTask;
        }




    }
}
