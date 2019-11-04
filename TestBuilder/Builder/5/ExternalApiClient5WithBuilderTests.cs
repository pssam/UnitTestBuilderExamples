using FluentAssertions;
using NUnit.Framework;
using Tests;

namespace TestBuilder.Builder._5
{
    [TestFixture]
    public class ExternalApiClient5WithBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_GetCount()
        {
            var builder = new ExternalApiBuilder5();
            var apiClient = builder.WithEmptyPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new ExternalApiBuilder5();
            var apiClient = builder.WithEmptyPost().WithDeletedPost().Build();

            var postCount = apiClient.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }



        [Test]
        public void Test_GetPosts()
        {
            var builder = new ExternalApiBuilder5();
            var apiClient = builder.WithPost("Title").Build();

            var posts = apiClient.GetPosts(builder.Tag);

            new[] { new PostView { Name = "Title" } }.Should().BeEquivalentTo(posts);
        }
    }
}