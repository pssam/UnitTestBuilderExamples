using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._3_ignore_deleted
{
    [TestFixture]
    public class ExternalApiClient3Tests
    {
        [Test]
        public void Test_GetCount_WhenNoDeletedRecords_ReturnAllRecords()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponse>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                        {Data = new GetPostResponse {Posts = new[] {new Post(), }}});

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
                .Setup(x => x.Execute<GetPostResponse>(
                    It.Is<IRestRequest>(request => request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                    {
                        Data = new GetPostResponse
                            {Posts = new [] {new Post(), new Post {IsDeleted = true},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient3(restClient.Object, config.Object);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}