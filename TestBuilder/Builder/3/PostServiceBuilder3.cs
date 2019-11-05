using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._3_ignore_deleted;
using Tests;

namespace TestBuilder.Builder._3
{
    internal class PostServiceBuilder3
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<Post> Posts { get; set; } = new List<Post>();

        public string Tag { get; set; } = "DefaultTag";

        public PostService3 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<GetPostResponse>(
                          It.Is<IRestRequest>(request => request.Resource == $"{BaseUrl}/explore/tags/{Tag}/"), Method.GET))
                      .Returns(
                          new RestResponse<GetPostResponse>
                              {Data = new GetPostResponse {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new PostService3(restClient.Object, config.Object);
        }

        public PostServiceBuilder3 WithPost()
        {
            Posts.Add(new Post());
            return this;
        }

        /// <summary>
        /// Метод формирует удалённые посты.
        /// </summary>
        /// <returns></returns>
        public PostServiceBuilder3 WithDeletedPost()
        {
            Posts.Add(new Post {IsDeleted = true});
            return this;
        }
    }
}