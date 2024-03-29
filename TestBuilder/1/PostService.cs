using RestSharp;
using Tests;

namespace TestBuilder._1
{
    public class PostService
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;

        public PostService(IRestClient client, IApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
            _client = client;
        }

        /// <summary>
        /// ����� ������ ������ � ���������� �������, ����� �������� ����� �������������� ����.
        /// ����� ���������� �� ����������.
        /// ����� ������� ������ �� ����������������� �����.
        /// </summary>
        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/?{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponse>(request, Method.GET);
            return posts.Data.Posts.Length;
        }
    }
}