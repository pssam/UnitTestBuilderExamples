using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._5_read_posts_add_map
{
    [TestFixture]
    public class ExternalApiClient5Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                        {Data = new GetPostResponseFull {Posts = new PostFull[] {new PostFull(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient5(restClient.Object, config.Object, MapperContext.Map);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_WhenHasDeletedRecords_CountOnlyActive()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                    {
                        Data = new GetPostResponseFull
                            {Posts = new PostFull[] {new PostFull(), new PostFull {IsDeleted = true},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient5(restClient.Object, config.Object, MapperContext.Map);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }


        [Test]
        public void Test_GetPosts()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                    {
                        Data = new GetPostResponseFull
                            {Posts = new[] {new PostFull {Title = "Title"},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient5(restClient.Object, config.Object, MapperContext.Map);

            var posts = apiClient.GetPosts("tag");

            new[] {new PostView {Name = "Title"}}.Should().BeEquivalentTo(posts);
        }
    }
}