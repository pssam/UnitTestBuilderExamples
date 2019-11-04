using NUnit.Framework;

namespace TestBuilder.Builder._1
{
    [TestFixture]
    public class ExternalApiClient1WithBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var apiClient = new ExternalApiBuilder1().WithEmptyPost().Build();

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}