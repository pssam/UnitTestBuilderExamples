using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using TestBuilder._7_add_feature_selection;
using Tests;

namespace TestBuilder.Builder._7
{
    /// <summary>
    /// �������� ����� �����������
    /// </summary>
    internal class PostServiceBuilder7
    {
        public string BaseUrl { get; set; } = "http://baseurl.com";

        public List<PostFull> Posts { get; set; } = new List<PostFull>();

        public string Tag { get; set; } = "DefaultTag";

        public PostService7 Build()
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

            return new PostService7(httpClient, config.Object,
                MapperContext.Map, new Mock<IFeatureConfig>().Object);
        }

        public PostServiceBuilder7 WithPost(string title = null)
        {
            Posts.Add(new PostFull { Title = title });
            return this;
        }

        public PostServiceBuilder7 WithDeletedPost()
        {
            Posts.Add(new PostFull {IsDeleted = true});
            return this;
        }
    }
}