using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    class Food : Point
    {
        public void Spawn(Point[] snake)
        {
            Random random = new Random();

            x = random.Next(2, 79);
            y = random.Next(0, 24);
        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write("σ");
        }
    }
}
