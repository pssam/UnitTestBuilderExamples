using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._5_read_posts_add_map;
using Tests;

namespace TestBuilder.Builder._5
{
    internal class ExternalApiBuilder5
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<PostFull> Posts { get; set; } = new List<PostFull>();

        public string Tag { get; set; } = "DefaultTag";

        public ExternalApiClient5 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<GetPostResponseFull>(
                          It.Is<IRestRequest>(request => request.Resource == $"{BaseUrl}/explore/tags/{Tag}/"), Method.GET))
                      .Returns(
                          new RestResponse<GetPostResponseFull>
                              {Data = new GetPostResponseFull() {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new ExternalApiClient5(restClient.Object, config.Object, MapperContext.Map);
        }

        public ExternalApiBuilder5 WithEmptyPost()
        {
            Posts.Add(new PostFull());
            return this;
        }

        public ExternalApiBuilder5 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }

        public ExternalApiBuilder5 WithPost(string title)
        {
            Posts.Add(new PostFull { Title = title });
            return this;
        }
    }
}