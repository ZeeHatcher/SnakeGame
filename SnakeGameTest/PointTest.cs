using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeGame;

namespace SnakeGameTest
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void SetPointTest()
        {
            Point point = new Point();
            Point target = new Point(2, 4);

            point.SetPoint(target);

            Assert.AreEqual(target.X, point.X);
            Assert.AreEqual(target.Y, point.Y);
        }
    }
}
