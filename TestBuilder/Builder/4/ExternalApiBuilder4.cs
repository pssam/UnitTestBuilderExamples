using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._3_ignore_deleted;
using Tests;

namespace TestBuilder.Builder._4
{
    internal class ExternalApiBuilder4
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<PostFull> Posts { get; set; } = new List<PostFull>();

        public string Tag { get; set; } = "DefaultTag";

        public ExternalApiClient3 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<GetPostResponseFull>(
                          It.Is<IRestRequest>(request => request.Resource == $"{BaseUrl}/explore/tags/{Tag}/"), Method.GET))
                      .Returns(
                          new RestResponse<GetPostResponseFull>
                              {Data = new GetPostResponseFull() {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new ExternalApiClient3(restClient.Object, config.Object);
        }

        public ExternalApiBuilder4 WithEmptyPost()
        {
            Posts.Add(new PostFull());
            return this;
        }

        public ExternalApiBuilder4 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }
    }
}