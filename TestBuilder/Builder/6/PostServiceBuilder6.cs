using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using TestBuilder._6_switch_to_HttpClient;
using Tests;

namespace TestBuilder.Builder._6
{
    /// <summary>
    /// Поменялся клиент.
    /// Исправляем только билдер
    /// </summary>
    internal class PostServiceBuilder6
    {
        public string BaseUrl { get; set; } = "http://baseurl.com";

        public List<PostFull> Posts { get; set; } = new List<PostFull>();

        public string Tag { get; set; } = "DefaultTag";

        public PostService6 Build()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(request =>
                        request.RequestUri.ToString() == $"{BaseUrl}/explore/tags/{Tag}/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new GetPostResponseFull
                        {Posts = Posts.ToArray()}))
                });
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns(BaseUrl);

            return new PostService6(httpClient, config.Object, MapperContext.Map);
        }

        public PostServiceBuilder6 WithPost(string title = null)
        {
            Posts.Add(new PostFull { Title = title });
            return this;
        }

        public PostServiceBuilder6 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }
    }
}