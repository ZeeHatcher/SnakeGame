using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    class Food : Point
    {
        private int counter = 0;

        public void Spawn(Point[] snake)
        {
            counter = 0;

            Random random = new Random();

            x = random.Next(0, 79);
            y = random.Next(2, 24);
        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write("σ");
        }

        public bool CheckCollision(Point[] snake)
        {
            if (snake[0].X == x && snake[0].Y == y)
            {
                Spawn(snake);
                return true;
            }
            return false;
        }

        public void CountTimer(Point[] snake)
        {
            counter += 1;
            if (counter == 100)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(" ");
                Spawn(snake);
            }
        }
    }
}
