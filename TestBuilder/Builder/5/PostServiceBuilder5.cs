using System.Collections.Generic;
using Moq;
using RestSharp;
using TestBuilder._5_read_posts_add_map;
using Tests;

namespace TestBuilder.Builder._5
{
    /// <summary>
    /// ѕо€вилась нова€ зависимость.
    /// ѕросто создадим новый мок и передадим его клиенту.
    /// MapperContext.Map
    /// </summary>
    internal class PostServiceBuilder5
    {
        public string BaseUrl { get; set; } = "DefaultBaseUrl";

        public List<PostFull> Posts { get; set; } = new List<PostFull>();

        public string Tag { get; set; } = "DefaultTag";

        public PostService5 Build()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == $"{BaseUrl}/explore/tags/{Tag}/"), Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                        {Data = new GetPostResponseFull() {Posts = Posts.ToArray()}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new PostService5(restClient.Object, config.Object,
                MapperContext.Map);
        }

        public PostServiceBuilder5 WithPost(string title = null)
        {
            Posts.Add(new PostFull { Title = title });
            return this;
        }

        public PostServiceBuilder5 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }
    }
}