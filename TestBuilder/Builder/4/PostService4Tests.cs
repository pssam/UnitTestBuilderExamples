using NUnit.Framework;

namespace TestBuilder.Builder._4
{
    /// <summary>
    /// —ами тесты не мен€ютс€.
    /// </summary>
    [TestFixture]
    public class PostService4Tests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new PostServiceBuilder4();
            var service = builder.WithPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new PostServiceBuilder4();
            var service = builder.WithPost().WithDeletedPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}