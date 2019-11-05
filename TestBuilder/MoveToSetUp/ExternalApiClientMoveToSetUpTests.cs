using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using TestBuilder._7_add_feature_selection;
using Tests;

namespace TestBuilder.MoveToSetUp
{
    [TestFixture]
    public class ExternalApiClientMoveToSetUpTests
    {
        private HttpClient _httpClient;
        private IApiConfig _config;

        [SetUp]
        public void Setup()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync",
                       ItExpr.Is<HttpRequestMessage>(request =>
                           request.RequestUri.ToString() == "http://baseurl.com/explore/tags/Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords/?__a=1"),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage { Content = new StringContent("{posts:[{}]}") });
            _httpClient = new HttpClient(handler.Object);

            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync",
                       ItExpr.Is<HttpRequestMessage>(request =>
                           request.RequestUri.ToString() == "http://baseurl.com/explore/tags/Test_GetCount_WhenHasDeletedRecords_CountOnlyActive/?__a=1"),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage
                       { Content = new StringContent("{posts:[{}, {IsDeleted: true}]}") });

            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync",
                       ItExpr.Is<HttpRequestMessage>(request =>
                           request.RequestUri.ToString() == "http://baseurl.com/explore/tags/Test_GetPosts/?__a=1"),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(new HttpResponseMessage
                       { Content = new StringContent("{posts:[{Title: 'Title'}]}") });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");
            _config = config.Object;
        }

        [Test]
        public void Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords()
        {
            var apiClient = new ExternalApiClient7(_httpClient, _config, MapperContext.Map, new Mock<IFeatureConfig>().Object);

            var postCount = apiClient.GetPostsCount("Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords");

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_WhenHasDeletedRecords_CountOnlyActive()
        {
            var apiClient = new ExternalApiClient7(_httpClient, _config, MapperContext.Map, new Mock<IFeatureConfig>().Object);

            var postCount = apiClient.GetPostsCount("Test_GetCount_WhenHasDeletedRecords_CountOnlyActive");

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetPosts()
        {
            var apiClient = new ExternalApiClient7(_httpClient, _config, MapperContext.Map, new Mock<IFeatureConfig>().Object);

            var posts = apiClient.GetPosts("Test_GetPosts");

            new[] {new PostView {Name = "Title"}}.Should().BeEquivalentTo(posts);
        }
    }
}