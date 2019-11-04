using System.Linq;
using System.Net.Http;
using AutoMapper;
using Newtonsoft.Json;
using Tests;

namespace TestBuilder._7_add_feature_selection
{
    public class ExternalApiClient7
    {
        private readonly HttpClient _client;
        private readonly IApiConfig _apiConfig;
        private readonly IMapper _mapper;
        private readonly IFeatureConfig _featureConfig;

        public ExternalApiClient7(HttpClient client, IApiConfig apiConfig, IMapper mapper, IFeatureConfig featureConfig)
        {
            _apiConfig = apiConfig;
            _mapper = mapper;
            _featureConfig = featureConfig;
            _client = client;
        }

        public int GetPostsCount(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(response.Content.ReadAsStringAsync().Result);
            return postsResponse.Posts.Count(post => _featureConfig.ShouldShowDeletedPosts || !post.IsDeleted);
        }

        public PostView[] GetPosts(string tag)
        {
            var response = _client.GetAsync($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1").Result;
            var postsResponse = JsonConvert.DeserializeObject<GetPostResponseFull>(response.Content.ReadAsStringAsync().Result);
            return postsResponse.Posts.Where(post => _featureConfig.ShouldShowDeletedPosts || !post.IsDeleted)
                                .Select(x => _mapper.Map<PostView>(x)).ToArray();
        }
    }
}