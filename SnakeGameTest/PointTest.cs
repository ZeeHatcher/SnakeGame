using System.Collections.Generic;
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

    [TestClass]
    public class FoodTest
    {
        [TestMethod]
        public void setFoodSpawnTest()
        {
            Food food = new Food();
            List<Point> snake = new List<Point> { new Point(0, 2), new Point(0, 2), new Point(0, 2) };
            food.Spawn(snake);
            Assert.IsTrue(food.X > 0);
            Assert.IsTrue(food.X < 79);
            Assert.IsTrue(food.Y > 2);
            Assert.IsTrue(food.Y < 24);
        }

        [TestMethod]
        public void FoodChildrenTest()
        {
            Food food = new Food();
            Assert.AreEqual(food.X, 0);
            Assert.AreEqual(food.Y, 0);
        }
    }
   
}
