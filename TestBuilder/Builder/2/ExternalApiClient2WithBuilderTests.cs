using NUnit.Framework;
using TestBuilder.Builder._1;

namespace TestBuilder.Builder._2
{
    [TestFixture]
    public class ExternalApiClient2WithBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiBuilder2();
            var apiClient = builder.WithEmptyPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}