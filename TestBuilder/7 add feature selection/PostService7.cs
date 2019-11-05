using System.Linq;
using System.Net.Http;
using AutoMapper;
using Newtonsoft.Json;
using Tests;

namespace TestBuilder._7_add_feature_selection
{
    public class PostService7
    {
        private readonly HttpClient _client;
        private readonly IApiConfig _apiConfig;
        private readonly IMapper _mapper;
        private readonly IFeatureConfig _featureConfig;

        public PostService7(HttpClient client, IApiConfig apiConfig, IMapper mapper, IFeatureConfig featureConfig)
        {
            _apiConfig = apiConfig;
            _mapper = mapper;
            _featureConfig = featureConfig;
            _client = client;
        }

        /// <summary>
        /// Требования продолжают меняться...
        /// Выяснилось, что некоторые заказчики хотят видеть удалённые посты.
        /// Решено ввести ещё один конфиг,
        /// который будет определять считать их или нет.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int GetPostsCount(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(content);
            return postsResponse
                   .Posts
                   .Count(post => _featureConfig.ShouldShowDeletedPosts ||
                                  !post.IsDeleted);
        }

        public PostView[] GetPosts(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(content);
            return postsResponse
                   .Posts
                   .Where(post => _featureConfig.ShouldShowDeletedPosts || !post.IsDeleted)
                   .Select(x => _mapper.Map<PostView>(x))
                   .ToArray();
        }
    }
}