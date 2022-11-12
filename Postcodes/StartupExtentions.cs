using Postcodes.Services;

namespace Postcodes
{
    public static class StartupExtentions
    {
        public static WebApplication RunApiHealthCheck(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var api = scope.ServiceProvider.GetRequiredService<IPostcodeApiService>();
                api.RunHealthCheck();
            }
            return app;
        }
    }
}
