namespace CityInfo.Services
{
    public interface ISenderService
    {
        void Send(string subject, string message);
    }
}
