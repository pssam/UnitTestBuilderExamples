using Moq;
using NUnit.Framework;
using RestSharp;
using Tests;

namespace TestBuilder._4_changed_API
{
    [TestFixture]
    public class PostService4Tests
    {
        /// <summary>
        /// ���� ������ ������ �� ��������, �� ���� ����� ���������� ������������.
        /// ������ ��� ��� �� ����������� � ����� �����,
        /// ����������� �������� ������������ �� ����.
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

            var service = new PostService4(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// �� ������ �� ��, ��� ����� ����� ������,
        /// ���� ��������, � ������ ���,
                                                                                            /// ������ ��� � ����� ��������� ��� � �����.
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

            var service = new PostService4(restClient.Object, config.Object);

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}