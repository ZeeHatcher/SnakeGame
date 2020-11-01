using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Reflection;

namespace SnakeGame
{
    class Food : Point
    {
        private int counter = 0;
        private ConsoleColor color;
        SoundPlayer soundPlayer;

        public Food()
            : base()
        {
            color = ConsoleColor.White;
            soundPlayer = new SoundPlayer(Assembly.GetExecutingAssembly().GetManifestResourceStream("SnakeGame.pickup_normal.wav"));
        }

        public Food(ConsoleColor color)
            : this()
        {
            this.color = color;
        }

        public void Spawn(List<Point> snake)
        {
            counter = 0;

            Random random = new Random();

            x = random.Next(0, 79);
            y = random.Next(2, 24);
        }

        
        public override void Render()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write("σ");
        }

        public bool CheckCollision(List<Point> snake)
        {
            if (snake[0].X == x && snake[0].Y == y)
            {
                Spawn(snake);
                soundPlayer.Play();
                return true;
            }
            return false;
        }
    
    }
}
