using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Tests;

namespace TestBuilder._7_add_feature_selection
{
    [TestFixture]
    public class PostService7Tests
    {
        /// <summary>
        /// Новый конфиг - новая зависимость.
        /// Исправляем все тесты...
        /// </summary>
        [Test]
        public void Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(request =>
                        request.RequestUri.ToString() ==
                        "http://baseurl.com/explore/tags/tag/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{posts:[{}]}")
                });
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");

            var service = new PostService7(httpClient, config.Object,
                MapperContext.Map, new Mock<IFeatureConfig>().Object);

            // Act
            var postCount = service.GetPostsCount("tag");

            // Assert
            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_WhenHasDeletedRecords_CountOnlyActive()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(request =>
                        request.RequestUri.ToString() == "http://baseurl.com/explore/tags/tag/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{posts:[{}, {IsDeleted: true}]}")
                });
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");

            var service = new PostService7(httpClient, config.Object,
                MapperContext.Map, new Mock<IFeatureConfig>().Object);

            // Act
            var postCount = service.GetPostsCount("tag");

            // Assert
            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetPosts()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(request =>
                        request.RequestUri.ToString() == "http://baseurl.com/explore/tags/tag/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                    {Content = new StringContent("{posts:[{Title: 'Title'}]}")});
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");

            var service = new PostService7(httpClient, config.Object, MapperContext.Map,
                new Mock<IFeatureConfig>().Object);

            // Act
            var posts = service.GetPosts("tag");

            // Assert
            new[] {new PostView {Name = "Title"}}.Should().BeEquivalentTo(posts);
        }
    }
}