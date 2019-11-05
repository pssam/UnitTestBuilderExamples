using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._2_bug_in_URL
{
    [TestFixture]
    public class PostService2Tests
    {
        /// <summary>
        /// Юнит тест становится ещё сложнее из-за того,
        /// что нам надо проверять урл, к которому происходит запрос.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponse>(
                    It.Is<IRestRequest>(request =>
                        request.Resource == "baseUrl/explore/tags/tag/"),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                        {Data = new GetPostResponse {Posts = new Post[] {new Post(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService2(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}