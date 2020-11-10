using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;

namespace SnakeGame
{
    internal class Program
    {
        private static int CONSOLE_WIDTH_LIMIT = 79;
        private static int CONSOLE_HEIGHT_LIMIT = 24;
        private static int SPEED_LIMIT = 30;
        private static ConsoleKeyInfo consoleKey; // holds whatever key is pressed
        private static SoundPlayer menuSoundPlayer, hitSoundPlayer;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Assembly assembly = Assembly.GetExecutingAssembly();
            menuSoundPlayer = new SoundPlayer(assembly.GetManifestResourceStream("SnakeGame.menu.wav"));
            hitSoundPlayer = new SoundPlayer(assembly.GetManifestResourceStream("SnakeGame.hit.wav"));

            // set console size
            Console.SetWindowSize(CONSOLE_WIDTH_LIMIT + 2, CONSOLE_HEIGHT_LIMIT + 2);

            //Initialize text to print on screen
            string[] menuOptions = new string[4] { "Play", "High Score", "Help", "Quit" };
            
            //Main menu loop
            bool exitGame = false;
            while (!exitGame)
            {
                //Show main menu text
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 3, CONSOLE_HEIGHT_LIMIT / 5 + (2 * i));
                    Console.WriteLine((i + 1) + ") " + menuOptions[i]);
                }
                //Get user input
                consoleKey = Console.ReadKey(true);

                menuSoundPlayer.Play();
                Console.Clear();

                //Process user input
                switch (consoleKey.Key)
                {
                    //Start game
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        StartGame();
                        break;
                    //Show highscore
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ShowScoreBoard();
                        break;
                    //Show help screen
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ShowHelpScreen();
                        break;
                    //Exit game
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        exitGame = true;
                        break;
                }
                Console.Clear();
            }
        }

        private static void ShowHelpScreen()
        {
            try
            {
                //Get text for help screen
                Assembly assembly = Assembly.GetExecutingAssembly();
                StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("SnakeGame.help_screen.txt"));
                String line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ", Help screen unavailable.");
            }
            Console.SetCursorPosition(0, CONSOLE_HEIGHT_LIMIT);
            Console.WriteLine("Press any key to return back to the main menu.");
            Console.ReadKey();

        }

        private static void ShowScoreBoard()
        {
            Console.WriteLine("--HIGHSCORE BOARD--");
            try
            {
                StreamReader sr = new StreamReader("highscore.txt");
                String line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("No scores available currently.");
            }
            Console.WriteLine("\nPress any key to return back to the main menu.");
            Console.ReadKey();
            return;
        }

        private static void StartGame()
        {
            //display this char on the console as the obstacle
            char obstacle = '|';
            //set the timer to 10 seconds
            int seconds = 10 * 1000;
            bool gameLive = true;

            //score threshold
            int limit = 2;
            int life = 1;
            Dictionary<int, int> obstacles = new Dictionary<int, int>();

            bool gamePause = false;
            //end game String (Win or Lose statement)
            String end_condition = "Game Over";

            // pause game string
            string[] pauseMessage = new string[3] { "Paused!", "Press any key to resume", "Press ESC to quit" };

            // score
            int score = 0;

            // location info & display
            List<Point> snake = new List<Point> { new Point(0, 2), new Point(0, 2), new Point(0, 2) };
            int dx = 1, dy = 0;

            System.Random random = new System.Random();
            //create random number for the obstacle within the console
            //minus the width limit by 1 for space for the second character of the obstacle
            int obx = random.Next(CONSOLE_WIDTH_LIMIT - 1);
            int oby = random.Next(2, CONSOLE_HEIGHT_LIMIT);
            obstacles.Add(obx, oby);

            Food food = new Food(ConsoleColor.Red);
            Food specialFood = new Food(ConsoleColor.Yellow);
            Food saviour = new Food(ConsoleColor.Green);
            //Spawns the food object
            //food.Spawn(snake);
            //specialFood.Spawn(snake

            var timer = new System.Threading.Timer(state => ChangePositions(), null, 0, seconds);

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = SPEED_LIMIT;

            // whether to keep trails
            bool trail = false;

            //function called by timer to change the positions of obstacles, food as well as special food
            void ChangePositions()
            {
                //loop through the dictionary to clear all the obstacles
                foreach (KeyValuePair<int, int> i in obstacles.ToArray())
                {
                    Console.SetCursorPosition(i.Key, i.Value);
                    Console.Write(' ');
                    Console.SetCursorPosition(i.Key + 1, i.Value);
                    Console.Write(' ');
                    obstacles.Remove(i.Key);
                    //i.Key = random.Next(consoleWidthLimit);
                    //i.Value = random.Next(2, consoleHeightLimit);

                    int x = random.Next(CONSOLE_WIDTH_LIMIT);
                    int y = random.Next(2, CONSOLE_HEIGHT_LIMIT);

                    //if the newly generated random position clashes with the snake
                    //generate a new position
                    foreach (Point p in snake)
                    {
                        while (p.X == x || p.X == x + 1 || obstacles.ContainsKey(x))
                        {
                            x = random.Next(CONSOLE_WIDTH_LIMIT - 1);
                        }

                        while (p.Y == y)
                        {
                            y = random.Next(2, CONSOLE_HEIGHT_LIMIT);
                        }
                    }
                    //replace the old positions with new positions
                    obstacles.Add(x, y);
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

                if (!saviour.CheckCollision(snake))
                {
                    Console.SetCursorPosition(saviour.X, saviour.Y);
                    Console.Write(' ');
                }

                //spawn the food object
                food.Spawn(snake);
                specialFood.Spawn(snake);
                saviour.Spawn(snake);
            }

            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = ConsoleColor.White;
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

                //check if the snake touched the obstacle
                foreach (Point p in snake)
                {
                    foreach (KeyValuePair<int, int> i in obstacles.ToArray())
                    {
                        if (p.Y == i.Value)
                        {
                            if (p.X == i.Key || p.X == i.Key + 1)
                            {
                                life -= 1;
                                if (life == 0)
                                {
                                    end_condition = "You suffocated in the obstacles!";
                                }
                                hitSoundPlayer.Play();
                                ChangePositions();
                            }
                        }
                    }
                }

                if (life == 0)
                {
                    gameLive = false;
                    break;
                }

                if (score >= limit)
                {
                    int x = random.Next(CONSOLE_WIDTH_LIMIT - 1);
                    int y = random.Next(2, CONSOLE_HEIGHT_LIMIT);

                    foreach (Point p in snake)
                    {
                        while (p.X == x || p.X == x + 1 || obstacles.ContainsKey(x))
                        {
                            x = random.Next(CONSOLE_WIDTH_LIMIT - 1);
                        }

                        while (p.Y == y)
                        {
                            y = random.Next(2, CONSOLE_HEIGHT_LIMIT);
                        }
                    }

                    obstacles.Add(x, y);

                    limit += 3;
                    delayInMillisecs = Math.Max(SPEED_LIMIT, delayInMillisecs - 10);
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
                if (snake[0].X > CONSOLE_WIDTH_LIMIT)
                    snake[0].X = 0;
                if (snake[0].X < 0)
                    snake[0].X = CONSOLE_WIDTH_LIMIT;

                snake[0].Y += dy;
                if (snake[0].Y > CONSOLE_HEIGHT_LIMIT)
                    snake[0].Y = 2; // 2 due to top spaces used for directions
                if (snake[0].Y < 2)
                    snake[0].Y = CONSOLE_HEIGHT_LIMIT;

                //check if the snake touched its body parts
                foreach (Point p in snake)
                {
                    if (p != snake[0] && p.Y == snake[0].Y && p.X == snake[0].X)
                    {
                        life--;
                        hitSoundPlayer.Play();
                        if (life == 0)
                        {
                            end_condition = "You tried to eat yourself!";
                        }
                        break;
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

                    delayInMillisecs = Math.Max(SPEED_LIMIT, delayInMillisecs - 10);
                }

                if (saviour.CheckCollision(snake))
                {
                    snake.Add(new Point(snake[snake.Count - 1]));
                    if (life < 3)
                    {
                        life += 1;
                    }
                    else
                    {
                        score += 1;
                    }
                }

                // render the food
                food.Render();
                specialFood.Render();
                saviour.Render();

                // render the snake
                foreach (Point p in snake)
                {
                    p.Render();
                }

                // render the score
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 4 * 3, 0);
                Console.Write("Score: " + score);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 4 * 3 + 10, 0);
                Console.Write("Life: " + life);

                foreach (KeyValuePair<int, int> i in obstacles.ToArray())
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(i.Key, i.Value);
                    Console.Write(obstacle);
                    Console.SetCursorPosition(i.Key + 1, i.Value);
                    Console.Write(obstacle);
                }

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

                while (gamePause)
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < pauseMessage.Length; i++)
                    {
                        Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 3, CONSOLE_HEIGHT_LIMIT / 2 + (2 * i));
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

            //Save the result
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Enter your name: ");
            String Name = Console.ReadLine();
            try
            {
                StreamWriter sw = new StreamWriter("highscore.txt", true, Encoding.ASCII);
                sw.WriteLine(Name + ": " + score);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Missing highscore.txt file.");
            }
            //Stop the timer
            timer.Dispose();
            //Write an end game screen in the middle
            Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 3, CONSOLE_HEIGHT_LIMIT / 2);
            // Write string depending if you lose or win
            Console.WriteLine(end_condition);
            //display the score
            Console.SetCursorPosition(CONSOLE_WIDTH_LIMIT / 3, CONSOLE_HEIGHT_LIMIT / 2 + 2);
            Console.WriteLine("Your Score is: " + score);
            Console.ReadKey();
        }
    }
}