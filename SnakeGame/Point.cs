using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SnakeGame
{
    class Point
    {
        protected int x, y;

        public Point()
            : this(0, 0)
        {
        }

        public Point(Point p)
        {
            this.x = p.X;
            this.y = p.Y;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public void SetPoint(Point point)
        {
            this.x = point.x;
            this.y = point.y;
        }

        public virtual void Render()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, y);
            Console.Write("*");
        }

        public virtual void RenderObs()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(x, y);
            Console.Write("|");
            Console.SetCursorPosition(x+1, y);
            Console.Write("|");
        }
    }
}
