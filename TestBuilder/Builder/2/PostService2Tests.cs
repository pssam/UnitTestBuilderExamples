using NUnit.Framework;

namespace TestBuilder.Builder._2
{
    [TestFixture]
    public class PostService2Tests
    {
        /// <summary>
        /// ���� ����������� �� ���������.
        /// ������ �� ������� ��� �� ������� ��� ��������.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var builder = new PostServiceBuilder2();
            var service = builder.WithPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }

        /// <summary>
        /// ����� �������� ���, ���� ��� ��� �����.
        /// </summary>
        [Test]
        public void Test_GetCount2()
        {
            var builder = new PostServiceBuilder2();
            builder.Tag = "new";
            var service = builder.WithPost().Build();

            var postCount = service.GetPostsCount(builder.Tag);

            Assert.AreEqual(1, postCount);
        }
    }
}