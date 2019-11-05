using NUnit.Framework;

namespace TestBuilder.Builder._1
{
    [TestFixture]
    public class PostService1Tests
    {

        /// <summary>
        /// ���� ������� � ��������.
        /// ���� � ������� ������������ ���� ����, �� �� ������ ��� ���������.
        /// </summary>
        [Test]
        public void Test_GetCount()
        {
            var service = new PostServiceBuilder1().WithPost().Build();

            var postCount = service.GetPostsCount("tag");

            Assert.AreEqual(1, postCount);
        }
    }
}