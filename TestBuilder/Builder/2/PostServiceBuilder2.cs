using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._2_bug_in_URL;
using Tests;

namespace TestBuilder.Builder._2
{
    /// <summary>
    /// Мы хотим проверять, что урл правильно формируется.
    /// Добавим тэг как зависимость и используем его.
    /// </summary>
    internal class PostServiceBuilder2
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<Post> Posts { get; set; } = new List<Post>();

        public string Tag { get; set; } = "DefaultTag";

        public PostService2 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x =>
                    x.Execute<GetPostResponse>(
                        It.Is<IRestRequest>(request =>
                            request.Resource == $"{BaseUrl}/explore/tags/{Tag}/"), 
                        Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                        {Data = new GetPostResponse {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new PostService2(restClient.Object, config.Object);
        }

        public PostServiceBuilder2 WithPost()
        {
            Posts.Add(new Post());
            return this;
        }
    }
}