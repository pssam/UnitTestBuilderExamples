using System.Configuration;

namespace Tests
{
    public class ApiConfig : IApiConfig
    {
        public string BaseUrl => ConfigurationManager.AppSettings["BaseUrl"];
    }
}