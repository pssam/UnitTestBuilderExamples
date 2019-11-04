using NUnit.Framework;
using TestBuilder.Builder._2;

namespace TestBuilder.Builder._3
{
    [TestFixture]
    public class ExternalApiClient3WithBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiBuilder3();
            var apiClient = builder.WithEmptyPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}