using NUnit.Framework;

namespace TestBuilder.Builder._1
{
    [TestFixture]
    public class ExternalApiClient1WithBuilderTests
    {

        /// <summary>
        /// “ест простой и пон€тный.
        /// ≈сли с сервера возвращаетс€ один пост, то мы должны его посчитать.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var apiClient = new ExternalApiClientBuilder1().WithPost().Build();

            var postCount = apiClient.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}