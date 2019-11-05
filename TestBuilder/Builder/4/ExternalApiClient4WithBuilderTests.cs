using NUnit.Framework;

namespace TestBuilder.Builder._4
{
    /// <summary>
    /// —ами тесты не мен€ютс€.
    /// </summary>
    [TestFixture]
    public class ExternalApiClient4WithBuilderTests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiClientBuilder4();
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiClientBuilder4();
            var apiClient = builder.WithPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}