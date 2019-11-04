using System.Linq;
using AutoMapper;
using RestSharp;
using Tests;

namespace TestBuilder._5_read_posts_add_map
{
    public class ExternalApiClient5
    {
        private readonly IRestClient _client;
        private readonly IApiConfig _apiConfig;
        private readonly IMapper _mapper;

        public ExternalApiClient5(IRestClient client, IApiConfig apiConfig, IMapper mapper)
        {
            _apiConfig = apiConfig;
            _mapper = mapper;
            _client = client;
        }

        public int GetPostsCount(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponseFull>(request, Method.GET);
            return posts.Data.Posts.Count(post => !post.IsDeleted);
        }

        public PostView[] GetPosts(string tag)
        {
            var request = new RestRequest($"{_apiConfig.BaseUrl}/explore/tags/{tag}/?__a=1");
            var posts = _client.Execute<GetPostResponseFull>(request, Method.GET);
            return posts.Data.Posts.Where(post => !post.IsDeleted).Select(x => _mapper.Map<PostView>(x)).ToArray();
        }
    }
}