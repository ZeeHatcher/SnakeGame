using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.CompilerServices;

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
            int seconds = 10 * 1000;
            bool gameLive = true;
            //score threshold
            int limit = 2;
            Dictionary<int, int> obstacles = new Dictionary<int, int>();
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed 
            //end game String (Win or Lose statement)
            String end_condition = "Game Over";
            //initial score
            int score = 0;

            // location info & display
            List<Point> snake = new List<Point> { new Point(0, 2), new Point(0, 2), new Point(0, 2) };
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;

            System.Random random = new System.Random();
            //create random number for the obstacle within the console
            int obx = random.Next(consoleWidthLimit);
            int oby = random.Next(2, consoleHeightLimit);
            obstacles.Add(obx, oby);
            Food food = new Food();

            var timer = new System.Threading.Timer((e) =>
            {
                //clear the previous obstacle and print the new obstacle in the new location
                foreach (KeyValuePair<int, int> i in obstacles.ToArray())
                {
                    Console.SetCursorPosition(i.Key, i.Value);
                    Console.Write(' ');
                    Console.SetCursorPosition(i.Key + 1, i.Value);
                    Console.Write(' ');
                    obstacles.Remove(i.Key);
                    //i.Key = random.Next(consoleWidthLimit);
                    //i.Value = random.Next(2, consoleHeightLimit);

                    int x = random.Next(consoleWidthLimit);
                    int y = random.Next(2, consoleHeightLimit);

                    //if the newly generated random position clashes with the snake
                    //generate a new position
                    foreach (Point p in snake)
                    {
                        while (p.X == x || p.X == x + 1 || obstacles.ContainsKey(x))
                        {
                            x = random.Next(consoleWidthLimit);
                        }

                        while (p.Y == y)
                        {
                            y = random.Next(2, consoleWidthLimit);
                        }
                    }
                    obstacles.Add(x, y);
                }

                //checks if the previous food is "eaten" by the snake
                //if not, the food will be cleared and a new food would be spawned
                if (!food.CheckCollision(snake))
                {
                    Console.SetCursorPosition(food.X, food.Y);
                    Console.Write(' ');
                }

                //Spawns food object
                food.Spawn(snake);
            }, null, 0, seconds);


            // clear to color
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 100;

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
                    foreach (KeyValuePair<int, int> i in obstacles)
                    {
                        if (p.Y == i.Value)
                        {
                            if (p.X == i.Key || p.X == i.Key + 1)
                            {
                                gameLive = false;
                                break;
                            }
                        }
                    }

                }

                if (score >= limit)
                {
                    int x = random.Next(consoleWidthLimit);
                    int y = random.Next(2, consoleHeightLimit);

                    foreach (Point p in snake)
                    {
                        while (p.X == x || p.X == x + 1 || obstacles.ContainsKey(x))
                        {
                            x = random.Next(consoleWidthLimit);
                        }

                        while (p.Y == y)
                        {
                            y = random.Next(2, consoleWidthLimit);
                        }
                     }
                
                obstacles.Add(x, y);

                limit += 3;
                if ((delayInMillisecs - 30) > 10)
                {
                    delayInMillisecs -= 30;
                }
                else
                {
                    delayInMillisecs = 10;
                }
            }


                //check winning condition
                if (score >= 20)
                {
                    end_condition = "You Win!";
                    gameLive = false;
                    break;
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

                foreach (KeyValuePair<int, int> i in obstacles.ToArray())
                {
                    Console.SetCursorPosition(i.Key, i.Value);
                    Console.Write(obstacle);
                    Console.SetCursorPosition(i.Key + 1, i.Value);
                    Console.Write(obstacle);
                }

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
                //Stop the timer
                timer.Dispose();
                //Write an end game screen in the middle
                Console.SetCursorPosition(consoleWidthLimit / 2, consoleHeightLimit / 2);
                // Write string depending if you lose or win
                Console.WriteLine(end_condition);
                //display the score
                Console.SetCursorPosition(consoleWidthLimit / 2, consoleHeightLimit / 2 + 2);
                Console.WriteLine("Your Score is: " + score);
                Console.ReadKey();
            
        }
    
    }

}
