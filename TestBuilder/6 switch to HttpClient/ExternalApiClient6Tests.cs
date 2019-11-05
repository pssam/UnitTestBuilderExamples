using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Tests;

namespace TestBuilder._6_switch_to_HttpClient
{
    [TestFixture]
    public class ExternalApiClient6Tests
    {
        /// <summary>
        /// Вот только подставить зависимость оказалось сложнее.
        /// Тесты уже не вмещаются в экран и их почти невозможно понять.
        /// Хотя логика тестов не поменялась.
        /// Может быть хоть комментарии помогут.
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

            var apiClient = new ExternalApiClient6(httpClient, config.Object, MapperContext.Map);

            // Act
            var postCount = apiClient.GetPostsCount("tag");

            //Assert
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
                        request.RequestUri.ToString() ==
                        "http://baseurl.com/explore/tags/tag/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                    {Content = new StringContent("{posts:[{}, {IsDeleted: true}]}")});
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");

            var apiClient = new ExternalApiClient6(httpClient, config.Object,
                MapperContext.Map);

            // Act
            var postCount = apiClient.GetPostsCount("tag");

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
                        request.RequestUri.ToString() ==
                        "http://baseurl.com/explore/tags/tag/?__a=1"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                    {Content = new StringContent("{posts:[{Title: 'Title'}]}")});
            var httpClient = new HttpClient(handler.Object);

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("http://baseurl.com");

            var apiClient = new ExternalApiClient6(httpClient, config.Object, MapperContext.Map);

            // Act
            var posts = apiClient.GetPosts("tag");

            // Assert
            new[] {new PostView {Name = "Title"}}.Should().BeEquivalentTo(posts);
        }
    }
}