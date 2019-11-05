using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._1;
using Tests;

namespace TestBuilder.Builder._1
{
    internal class PostServiceBuilder1
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// Собираем зависимости вместе.
        /// </summary>
        /// <returns></returns>
        public PostService Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x =>
                    x.Execute<GetPostResponse>(It.IsAny<IRestRequest>(),
                        Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                        {Data = new GetPostResponse {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new PostService(restClient.Object, config.Object);
        }

        /// <summary>
        /// Добавляем пост, который должен возвращаться с сервера.
        /// </summary>
        /// <returns></returns>
        public PostServiceBuilder1 WithPost()
        {
            Posts.Add(new Post());
            return this;
        }
    }
}