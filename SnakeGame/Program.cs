using System;
using System.Collections.Generic;
using System.Media;
using System.Reflection;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Assembly assembly = Assembly.GetExecutingAssembly();
            SoundPlayer menuSoundPlayer = new SoundPlayer(assembly.GetManifestResourceStream("SnakeGame.menu.wav"));
            SoundPlayer hitSoundPlayer = new SoundPlayer(assembly.GetManifestResourceStream("SnakeGame.hit.wav"));

            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            menuSoundPlayer.Play();

            //display this char on the console as the obstacle
            char obstacle = '|';
            //set the timer to 10 seconds
            int seconds = 10 * 1000;
            bool gameLive = true;
            bool gamePause = false;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed 
            
            //end game String (Win or Lose statement)
            String end_condition = "Game Over";

            // pause game string
            string[] pauseMessage = new string[3] { "Paused!", "Press any key to resume", "Press ESC to quit" };

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
            Food food = new Food(ConsoleColor.Red);
            Food specialFood = new Food(ConsoleColor.Yellow);
           
            var timer = new System.Threading.Timer((e) =>
            {//clear the previous obstacle and print the new obstacle in the new location
                Console.SetCursorPosition(obx, oby);
                Console.Write(' ');
                Console.SetCursorPosition(obx + 1, oby);
                Console.Write(' ');
                obx = random.Next(consoleWidthLimit);
                oby = random.Next(2, consoleHeightLimit);

                //if the newly generated random position clashes with the snake
                //generate a new position
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

                //checks if the previous food is "eaten" by the snake
                //if not, the food will be cleared and a new food would be spawned
                if (!food.CheckCollision(snake))
                {
                    Console.SetCursorPosition(food.X, food.Y);
                    Console.Write(' ');                  
                }

                if (!specialFood.CheckCollision(snake))
                {
                    Console.SetCursorPosition(specialFood.X, specialFood.Y);
                    Console.Write(' ');                  
                }

                //Spawns food object
                food.Spawn(snake);
                specialFood.Spawn(snake);
            }, null, 0, seconds);                

      
            // clear to color
            Console.BackgroundColor = ConsoleColor.Black;
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
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(snake[0].X, snake[0].Y);

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {

                        case ConsoleKey.UpArrow: //UP
                            if (dy == 1) break;

                            dx = 0;
                            dy = -1;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            if (dy == -1) break;

                            dx = 0;
                            dy = 1;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            if (dx == 1) break;

                            dx = -1;
                            dy = 0;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            if (dx == -1) break;

                            dx = 1;
                            dy = 0;
                            break;
                        case ConsoleKey.Escape: //END
                            gamePause = true;
                            menuSoundPlayer.Play();
                            break;
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

                //check if the snake touched the obstacle
                foreach (Point p in snake)
                {
                    if (p != snake[0] && p.Y == snake[0].Y && p.X == snake[0].X)
                    {
                        gameLive = false;
                        hitSoundPlayer.Play();
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
                            hitSoundPlayer.Play();
                            break;
                        }
                    }
                }

                //check winning condition
                if (score >= 20)
                {
                    end_condition = "You Win!";
                    gameLive = false;
                    break;
                }

                if (food.CheckCollision(snake))
                {
                    snake.Add(new Point(snake[snake.Count - 1]));
                    score += 1;
                }

                if (specialFood.CheckCollision(snake))
                {
                    snake.Add(new Point(snake[snake.Count - 1]));
                    score += 2;

                    delayInMillisecs = Math.Max(10, delayInMillisecs - 10);
                }


                // render the food
                food.Render();
                specialFood.Render();
                
                // render the snake
                foreach (Point p in snake)
                {
                    p.Render();
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

                while (gamePause)
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < pauseMessage.Length; i++)
                    {
                        Console.SetCursorPosition(consoleWidthLimit / 2, consoleHeightLimit / 2 + (2 * i));
                        Console.WriteLine(pauseMessage[i]);
                    }

                    if (Console.KeyAvailable)
                    {
                        consoleKey = Console.ReadKey(true);
                        switch (consoleKey.Key)
                        {
                            case ConsoleKey.Escape:
                                gameLive = false;
                                gamePause = false;
                                break;

                            default:
                                gamePause = false;
                                break;
                        }

                        menuSoundPlayer.Play();
                        Console.Clear();
                    }
                }

                Console.ForegroundColor = cc;

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
