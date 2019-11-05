using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._3_ignore_deleted
{
    [TestFixture]
    public class PostService3Tests
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

            var service = new PostService3(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// Нам приходится писать новый тест,
        /// который проверяет, что удалённые посты не учитываются.
        /// Код дублируется, а тесты становятся ещё непонятнее.
        /// </summary>
        [Test]
        public void Test_GetCount_WhenHasDeletedRecords_CountOnlyActive()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponse>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                    {
                        Data = new GetPostResponse
                            {Posts = new[] {new Post(), new Post {IsDeleted = true},}}
                    });

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService3(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}