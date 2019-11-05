using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._3_ignore_deleted;
using Tests;

namespace TestBuilder.Builder._3
{
    internal class ExternalApiClientBuilder3
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

        public ExternalApiClientBuilder3 WithPost()
        {
            Posts.Add(new PostFull());
            return this;
        }

        /// <summary>
        /// Метод формирует удалённые посты.
        /// </summary>
        /// <returns></returns>
        public ExternalApiClientBuilder3 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }
    }
}