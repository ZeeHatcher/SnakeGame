using System;
using System.Diagnostics;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // display this char on the console during the game
            char ch = '*';
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // location info & display
            Point[] snake = { new Point(0, 2), new Point(0, 2), new Point(0, 2) };
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;

            // food
            Food food = new Food();
            food.Spawn(snake);

            // clear to color
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;

            // whether to keep trails
            bool trail = false;

            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(snake[0].X, snake[0].Y);
                Console.ForegroundColor = cc;

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {
                      
                        case ConsoleKey.UpArrow: //UP
                            dx = 0;
                            dy = -1;
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            dx = 0;
                            dy = 1;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            dx = -1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            dx = 1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // find the tail in the console grid & erase the character there if don't want to see the trail
                Console.SetCursorPosition(snake[snake.Length - 1].X, snake[snake.Length - 1].y);
                if (trail == false)
                    Console.Write(' ');

                // set position of snake body parts
                for (int i = snake.Length - 1; i > 0; i--)
                {
                    snake[i].SetPoint(snake[i - 1]);
                }

                // calculate the new position
                // note x set to 0 because we use the whole width, but y set to 1 because we use top row for instructions
                snake[0].X += dx;
                if (snake[0].X > consoleWidthLimit)
                    snake[0].X = 0;
                if (snake[0].X < 0)
                    snake[0].X = consoleWidthLimit;

                snake[0].Y += dy;
                if (snake[0].Y > consoleHeightLimit)
                    snake[0].Y = 2; // 2 due to top spaces used for directions
                if (snake[0].Y < 2)
                    snake[0].Y = consoleHeightLimit;

                food.CheckCollision(snake);
                food.CountTimer(snake);

                // render the food
                food.Render();
                
                // render the snake
                foreach (Point p in snake)
                {
                    Console.SetCursorPosition(p.X, p.Y);
                    Console.Write(ch);
                }

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
        }
    }
    
}
