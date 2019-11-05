using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._4_changed_API
{
    [TestFixture]
    public class ExternalApiClient4Tests
    {
        /// <summary>
        /// ’от€ логика тестов не мен€етс€, но сами тесты приходитс€ переписывать.
        /// ѕричЄм это уже не исправление в одном месте,
        /// исправлени€ начинают расползатьс€ по коду.
        /// </summary>
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
                        {Data = new GetPostResponseFull {Posts = new PostFull[] {new PostFull(),}}});

            var config = new Mock<IApiConfig>();
            config.Setup(x => x.BaseUrl).Returns("baseUrl");

            var apiClient = new ExternalApiClient4(restClient.Object, config.Object);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// Ќе смотр€ на то, что тесты очень похоже,
        /// один работает, а второй нет,
        /// потому что € забыл исправить тип в тесте.
        /// (GetPostResponse => GetPostResponseFull)
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

            var apiClient = new ExternalApiClient4(restClient.Object, config.Object);

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}