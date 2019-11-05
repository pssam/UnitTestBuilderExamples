using NUnit.Framework;

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
            var builder = new ExternalApiClientBuilder3();
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// Есть удалённый и неудалённые пост.
        /// Считаем только 1.
        /// </summary>
        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiClientBuilder3();
            var apiClient = builder.WithPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}