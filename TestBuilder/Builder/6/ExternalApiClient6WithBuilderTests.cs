using FluentAssertions;
using NUnit.Framework;
using Tests;

namespace TestBuilder.Builder._6
{
    [TestFixture]
    public class ExternalApiClient6WithBuilderTests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiClientBuilder6();
            var apiClient = builder.WithPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiClientBuilder6();
            var apiClient = builder.WithPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetPosts()
        {
            var builder = new ExternalApiClientBuilder6();
            var apiClient = builder.WithPost("Title").Build();

            var posts = apiClient.GetPosts(builder.Tag);

            new[] { new PostView { Name = "Title" } }.Should().BeEquivalentTo(posts);
        }
    }
}