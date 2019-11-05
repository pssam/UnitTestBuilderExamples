using NUnit.Framework;
using TestBuilder.Builder._1;

namespace TestBuilder.Builder._2
{
    [TestFixture]
    public class ExternalApiClient2WithBuilderTests
    {
        /// <summary>
        /// Тест практически не изменился.
        /// Теперь мы передаём тэг из билдера как параметр.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiClientBuilder2();
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// Можно поменять тэг, если нам это нужно.
        /// </summary>
        [Test]
        public void Test_GetCount2()
        {
            var builder = new ExternalApiClientBuilder2();
            builder.Tag = "new";
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}