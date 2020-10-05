﻿using System;
using System.Collections.Generic;
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
            //display this char on the console as the obstacle
            char obstacle = '|';
            //set the timer to 10 seconds
            int seconds = 5 * 1000;
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // score
            int score = 0;

            // location info & display
            List<Point> snake = new List<Point> { new Point(0, 2), new Point(0, 2), new Point(0, 2) };
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;

            System.Random random = new System.Random();
            //create random number for the obstacle within the console
            int obx = random.Next(consoleWidthLimit);
            int oby = random.Next(consoleHeightLimit);

            var timer = new System.Threading.Timer((e) =>
            {//clear the previous obstacle and print the new obstacle in the new location
                Console.SetCursorPosition(obx, oby);
                Console.Write(' ');
                Console.SetCursorPosition(obx + 1, oby);
                Console.Write(' ');
                obx = random.Next(consoleWidthLimit);
                oby = random.Next(2, consoleHeightLimit);
                foreach (Point p in snake)
                {
                    if (p.Y == oby)
                    {
                        if (p.X == obx || p.X == obx + 1)
                        {
                            obx = random.Next(consoleWidthLimit);
                            oby = random.Next(2, consoleHeightLimit);
                        }
                    }
                }
            }, null, 0, seconds);

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

                //check if the snake touched the obstacle
                foreach (Point p in snake)
                {
                    if (p.Y == oby)
                    {
                        if (p.X == obx || p.X == obx + 1)
                        {
                            gameLive = false;
                            break;
                        }
                    }
                }

                // find the tail in the console grid & erase the character there if don't want to see the trail
                Console.SetCursorPosition(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
                if (trail == false)
                    Console.Write(' ');

                // set position of snake body parts
                for (int i = snake.Count - 1; i > 0; i--)
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

                if (food.CheckCollision(snake))
                {
                    snake.Add(new Point(snake[snake.Count - 1]));
                    score += 1;
                }

                food.CountTimer(snake);

                // render the food
                food.Render();
                
                // render the snake
                foreach (Point p in snake)
                {
                    Console.SetCursorPosition(p.X, p.Y);
                    Console.Write(ch);
                }

                // render the score
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(85, 0);
                Console.Write("Score: " + score);

                Console.SetCursorPosition(obx, oby);
                Console.Write(obstacle);
                Console.SetCursorPosition(obx + 1, oby);
                Console.Write(obstacle);

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
        }
    }

}
