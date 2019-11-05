using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._1
{
    [TestFixture]
    public class PostService1Tests
    {
        /// <summary>
        /// Не смотря на то, что код, который проверяется достаточно простой и короткий,
        /// Юните тест для его проверки получается довольно громоздким и непонятным.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var restClient = new Mock<IRestClient>();
            restClient
                .Setup(x => x.Execute<GetPostResponse>(It.IsAny<IRestRequest>(),
                    Method.GET))
                .Returns(
                    new RestResponse<GetPostResponse>
                        {Data = new GetPostResponse {Posts = new Post[] {new Post(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var service = new PostService(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}