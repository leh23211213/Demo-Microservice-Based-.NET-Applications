using App.Services.EmailAPI.Messaging;

namespace App.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer AzureServiceBusConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusCunsumer(this IApplicationBuilder app)
        {
            AzureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            AzureServiceBusConsumer.Start();
        }
        private static void OnStop()
        {
            AzureServiceBusConsumer.Stop();
        }
    }
}