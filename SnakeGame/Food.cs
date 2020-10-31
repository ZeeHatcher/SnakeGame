using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    class Food : Point
    {
        private int counter = 0;

        public void Spawn(List<Point> snake)
        {
            counter = 0;

            Random random = new Random();

            x = random.Next(0, 79);
            y = random.Next(2, 24);
        }

        
        public override void Render()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write("σ");
        }

        public bool CheckCollision(List<Point> snake)
        {
            if (snake[0].X == x && snake[0].Y == y)
            {
                Spawn(snake);
                return true;
            }
            return false;
        }
    
    }
}
