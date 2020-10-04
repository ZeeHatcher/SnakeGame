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
    }
}
