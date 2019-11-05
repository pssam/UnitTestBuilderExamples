using System.Linq;
using System.Net.Http;
using AutoMapper;
using Newtonsoft.Json;
using Tests;

namespace TestBuilder._6_switch_to_HttpClient
{
    public class ExternalApiClient6
    {
        private readonly HttpClient _client;
        private readonly IApiConfig _apiConfig;
        private readonly IMapper _mapper;

        public ExternalApiClient6(HttpClient client, IApiConfig apiConfig, IMapper mapper)
        {
            _apiConfig = apiConfig;
            _mapper = mapper;
            _client = client;
        }

        /// <summary>
        /// Проблемка!
        /// Выясняется, что библиотека, которую мы использовали, содержит серьёзный баг,
        /// а её поддержка прекратилось.
        /// Принято решение отказаться от неё и использовать другую библиотеку.
        /// Код изменился не сильно, поэтому проблем возникнуть не должно.
        /// Поменялся тип клиента и то, как десериализуется Json.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int GetPostsCount(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(response.Content.ReadAsStringAsync().Result);
            return postsResponse.Posts.Count(post => !post.IsDeleted);
        }

        public PostView[] GetPosts(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(response.Content.ReadAsStringAsync().Result);
            return postsResponse.Posts.Where(post => !post.IsDeleted).Select(x => _mapper.Map<PostView>(x)).ToArray();
        }
    }
}