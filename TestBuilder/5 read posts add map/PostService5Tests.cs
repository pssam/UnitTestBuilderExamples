using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._5_read_posts_add_map
{
    /// <summary>
    /// ќп€ть приходитс€ исправл€ть все тесты.
    /// ѕлюс по€вл€етс€ новый тест дл€ проверки нового метода.
    /// </summary>
    [TestFixture]
    public class PostService5Tests
    {
        [Test]
        public void Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                        {Data = new GetPostResponseFull {Posts = new[] {new PostFull(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService5(restClient.Object, config.Object,
                MapperContext.Map);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_WhenHasDeletedRecords_CountOnlyActive()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                    {
                        Data = new GetPostResponseFull
                            {Posts = new[] {new PostFull(), new PostFull {IsDeleted = true},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService5(restClient.Object, config.Object, MapperContext.Map);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// ¬ тесте уже можно запутатьс€.
        /// „то именно провер€етс€, какие параметры важны становитс€ не пон€тно.
        /// </summary>
        [Test]
        public void Test_GetPosts()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                    {
                        Data = new GetPostResponseFull
                            {Posts = new[] {new PostFull {Title = "Title"},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService5(restClient.Object, config.Object,
                MapperContext.Map);

            var posts = service.GetPosts("tag");

            new[] {new PostView {Name = "Title"}}.Should().BeEquivalentTo(posts);
        }
    }
}