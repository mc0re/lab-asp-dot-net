using System.Diagnostics;

namespace CityInfo.Services
{
    public class LocalMailService : ISenderService
    {
        private readonly string mTo = Startup.Configuration["mailSettings:to"];

        private readonly string mFrom = Startup.Configuration["mailSettings:from"];


        void ISenderService.Send(string subject, string message)
        {
            Debug.WriteLine($"Sending mail from {mFrom} to {mTo} '{subject}': {message}");
        }
    }
}
