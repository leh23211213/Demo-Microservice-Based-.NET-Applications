
namespace App.Services.RewardAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseAzureServiceBusCunsumer(this IApplicationBuilder app)
        {


            return app;
        }

        private static void OnStart()
        {

        }
        private static void OnStop()
        {
        }
    }
}