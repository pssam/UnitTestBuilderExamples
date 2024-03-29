using RestSharp;
using Tests;

namespace TestBuilder._2_bug_in_URL
{
    public class PostService2
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;

        public PostService2(IRestClient client, IApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
            _client = client;
        }

        /// <summary>
        /// �� ������ �� ���� �����, �� ����� ������ � ����.
        /// �����, � �������� �� ���������� �������� ������ ���� �������.
        /// </summary>
        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponse>(request, Method.GET);
            return posts.Data.Posts.Length;
        }
    }
}