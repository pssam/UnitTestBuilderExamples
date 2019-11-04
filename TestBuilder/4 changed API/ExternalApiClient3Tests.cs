using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._4_changed_API
{
    [TestFixture]
    public class ExternalApiClient3Tests
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
                        {Data = new GetPostResponseFull {Posts = new PostFull[] {new PostFull(), }}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient3(restClient.Object, config.Object);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}