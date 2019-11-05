using NUnit.Framework;

namespace TestBuilder.Builder._3
{
    [TestFixture]
    public class PostService3ests
    {
        [Test]
        public void Test_GetCount()
        {
            var builder = new PostServiceBuilder3();
            var service = builder.WithPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// Есть удалённый и неудалённые пост.
        /// Считаем только 1.
        /// </summary>
        [Test]
        public void Test_GetCount_IgnoresDeleted()
        {
            var builder = new PostServiceBuilder3();
            var service = builder.WithPost().WithDeletedPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}