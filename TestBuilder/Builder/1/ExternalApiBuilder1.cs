using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._1;
using Tests;

namespace TestBuilder.Builder._1
{
    internal class ExternalApiBuilder1
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<Post> Posts { get; set; } = new List<Post>();

        public ExternalApiClient1 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<GetPostResponse>(It.IsAny<IRestRequest>(), Method.GET))
                      .Returns(
                          new RestResponse<GetPostResponse>
                              { Data = new GetPostResponse { Posts = Posts.ToArray() }});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new ExternalApiClient1(restClient.Object, config.Object);
        }

        public ExternalApiBuilder1 WithEmptyPost()
        {
            Posts.Add(new Post());
            return this;
        }
    }
}