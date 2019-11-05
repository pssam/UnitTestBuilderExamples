using FluentAssertions;
using NUnit.Framework;
using TestBuilder.Builder._7;
using Tests;

namespace TestBuilder.FinalComparison
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new PostServiceBuilder7();
            var service = builder.WithPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new PostServiceBuilder7();
            var service = builder.WithPost().WithDeletedPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetPosts()
        {
            var builder = new PostServiceBuilder7();
            var service = builder.WithPost("Title").Build();

            var posts = service.GetPosts(builder.Tag);

            new[] { new PostView { Name = "Title" } }.Should().BeEquivalentTo(posts);
        }
    }
}