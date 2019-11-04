using NUnit.Framework;
using TestBuilder.Builder._3;

namespace TestBuilder.Builder._4
{
    [TestFixture]
    public class ExternalApiClient4WithBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiBuilder4();
            var apiClient = builder.WithEmptyPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiBuilder4();
            var apiClient = builder.WithEmptyPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}