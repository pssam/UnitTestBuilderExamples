using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._3_ignore_deleted
{
    [TestFixture]
    public class ExternalApiClient3TestsFail
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponseFull>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponseFull>
                        {Data = new GetPostResponseFull { Posts = new [] {new PostFull(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient3(restClient.Object, config.Object);

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
                            { Posts = new PostFull[] { new PostFull(), new PostFull { IsDeleted = true }, } }
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient3(restClient.Object, config.Object);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}