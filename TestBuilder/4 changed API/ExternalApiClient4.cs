using RestSharp;
using Tests;

namespace TestBuilder._4_changed_API
{
    public class ExternalApiClient4
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;

        public ExternalApiClient4(IRestClient client, IApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
            _client = client;
        }

        /// <summary>
        /// ����������, ��� �� ������������ ������������ ����� ��� ������ � �������.
        /// ������ GetPostResponse ������������ GetPostResponseFull.
        /// �������� ��, ��������� ���������.
        /// </summary>
        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponseFull>(request, Method.GET);
            return posts.Data.Posts.Length;
        }
    }
}