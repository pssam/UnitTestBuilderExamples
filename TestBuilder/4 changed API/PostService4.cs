using System.Linq;
using RestSharp;
using Tests;

namespace TestBuilder._4_changed_API
{
    public class PostService4
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;

        public PostService4(IRestClient client, IApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
            _client = client;
        }

        /// <summary>
        /// ¬ы€снилось, что мы использовали неправильный класс дл€ ответа с сервера.
        /// ¬место GetPostResponse возвращаетс€ GetPostResponseFull.
        ///  азалось бы, маленькие изменени€.
        /// </summary>
        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponseFull>(request, Method.GET);
            return posts.Data.Posts.Count(post => !post.IsDeleted);
        }
    }
}