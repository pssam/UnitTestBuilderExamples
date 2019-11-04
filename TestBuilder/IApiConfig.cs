using System.Configuration;

namespace Tests
{
    public interface IApiConfig
    {
        string BaseUrl { get; }
    }
}