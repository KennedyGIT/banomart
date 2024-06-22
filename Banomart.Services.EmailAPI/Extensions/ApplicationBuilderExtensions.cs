using Banomart.Services.EmailAPI.Messaging;

namespace Banomart.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app) 
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IServiceBusConsumer>();
            var hostApplicationLifeTime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifeTime.ApplicationStarted.Register(OnStart);
            hostApplicationLifeTime.ApplicationStopping.Register(OnStop);

            return app;

        }

        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }
    }
}
