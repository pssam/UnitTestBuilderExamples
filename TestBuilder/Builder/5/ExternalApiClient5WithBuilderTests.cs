using FluentAssertions;
using NUnit.Framework;
using Tests;

namespace TestBuilder.Builder._5
{
    /// <summary>
    /// Существующие тесты не изменились.
    /// Допишем новый тест.
    /// </summary>
    [TestFixture]
    public class ExternalApiClient5WithBuilderTests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiClientBuilder5();
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiClientBuilder5();
            var apiClient = builder.WithPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetPosts()
        {
            var builder = new ExternalApiClientBuilder5();
            var apiClient = builder.WithPost("Title").Build();

            var posts = apiClient.GetPosts(builder.Tag);

            new[] { new PostView { Name = "Title" } }.Should().BeEquivalentTo(posts);
        }
    }
}