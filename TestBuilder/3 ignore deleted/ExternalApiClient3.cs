using System.Linq;
using RestSharp;
using Tests;

namespace TestBuilder._3_ignore_deleted
{
    public class ExternalApiClient3
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;

        public ExternalApiClient3(IRestClient client, IApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
            _client = client;
        }

        /// <summary>
        /// После короткого ручного тестирования выясняется,
        /// что сервис возвращает посты, которые были удалены и нам их не нужно считать.
        /// </summary>
        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponse>(request, Method.GET);
            return posts.Data.Posts.Count(post => !post.IsDeleted);
        }
    }
}