namespace MazeGame // :D
{
    public class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;

            int i = 0;
            int[] sizes = [13, 17, 25, 37, 53, 77];

            bool shouldExit = false;

            while (!shouldExit)
            {
                int h = sizes[i];
                int w = sizes[i];
                shouldExit = Methods.Game(h, w, i + 1);
                i++;
                if (i == 5)
                {
                    Console.Clear();
                    Methods.WriteAtCenter("You WIN!!!!!!! Congradulations.", 0);
                    shouldExit = true;
                }
            }
        }
    }
    public class Methods
    {
        public static void WriteAtCenter(string text, int linesUp)
        {
            Draw(text, (Console.WindowWidth - text.Length) / 2, (Console.WindowHeight / 2) - linesUp);
        }
        public static bool Game(int h, int w, int l)
        {
            int x = 0;
            int y = 0;

            bool[][] initMaze = new bool[h][];

            for (int i = 0; i < h; i++)
            {
                initMaze[i] = new bool[w];
                Array.Fill(initMaze[i], true);
            }

            bool[][] maze = GenerateMaze(h, w, w / 2, h / 2, initMaze, [0, 2]);
            initMaze[0][0] = false;

            if (DisplayMaze(maze, l))
            {
                return true;
            }

            const string player = "••";

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Draw(player, 2 * (x + 1), y + 1);

            while (true)
            {
                ConsoleKey keyInfo = Console.ReadKey(true).Key;
                if (keyInfo == ConsoleKey.Q)
                    return true;

                if (x == w - 1 && y == h - 1 && keyInfo == ConsoleKey.RightArrow)
                {
                    if (l != 5)
                    {
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Clear();
                        WriteAtCenter("You won!", 2);
                        WriteAtCenter("I DARE YOU . . .", 0);
                        WriteAtCenter("Try a HARDER maze.", -1);
                        WriteAtCenter("(Or, press Q to quit.)", -2);
                        keyInfo = Console.ReadKey(true).Key;
                        if (keyInfo == ConsoleKey.Q)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                int[] dirs = Methods.GetDirs(keyInfo);
                if (Methods.CanGo(x, y, h, w, dirs, maze, false))
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Methods.Draw("  ", 2 * (x + 1), y + 1);
                    x += dirs[0];
                    y += dirs[1];
                    Console.BackgroundColor = ConsoleColor.White;
                    Methods.Draw(player, 2 * (x + 1), y + 1);
                }
            }
        }
        public static void Draw(string? obj, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(obj);
        }
        public static int[] GetDirs(ConsoleKey keyInfo)
        {
            return keyInfo switch
            {
                ConsoleKey.LeftArrow => [-1, 0],
                ConsoleKey.RightArrow => [1, 0],
                ConsoleKey.UpArrow => [0, -1],
                ConsoleKey.DownArrow => [0, 1],
                _ => [0, 0],
            };
        }
        public static bool DisplayMaze(bool[][] maze, int l)
        {
            int neededHeight = maze.Length + 5;
            int neededWidth = (maze[0].Length + 2) * 2 + 11;

            if (l == 6)
            {
                neededWidth += 52;
            }

            while (Console.WindowHeight < neededHeight || Console.WindowWidth < neededWidth)
            {
                Console.Clear();
                WriteAtCenter("Console is not large enough for the maze.", 2);
                WriteAtCenter($"Console is{(Console.WindowHeight >= neededHeight ? " " : " not ")}tall enough.", 1);
                WriteAtCenter($"Console is{(Console.WindowWidth >= neededWidth ? " " : " not ")}wide enough.", 0);
                WriteAtCenter("Please make the terminal larger and press any key; press the Q key to quit.", -1);
                while (!Console.KeyAvailable) ;
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    return true;
                }
            }

            Console.Clear();

            int i = 0;

            for (; i < maze[0].Length + 2; i++)
            {
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.Write("██");
            }
            Console.WriteLine();

            i = 0;

            foreach (bool[] array in maze)
            {
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.Write("██");
                int j = 0;
                foreach (bool obj in array)
                {
                    if (obj)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write("██");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    j++;
                }
                if (i == maze.Length - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("->");
                    break;
                }
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }

                Console.Write("██");

                if (i == (int)maze.Length / 2)
                {
                    Console.Write($"\tLevel {l}");
                    if (l == 6)
                    {
                        Console.Write(": The hardest level that can fit on the screen . . .");
                    }
                }

                i++;
                Console.WriteLine();
            }
            for (i = 0; i < maze[0].Length + 2; i++)
            {
                if (i % 2 == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.Write("██");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n\nPress the Q key to quit!\nDO NOT resize the terminal.");
            return false;
        }
        public static bool[][] GenerateMaze(int h, int w, int x, int y, bool[][] maze, int[] lastDirs)
        {
            Random random = new();

            maze[y][x] = false;
            int[][] initDirs =
            [
                [0, 2],
                [-2, 0],
                [0, -2],
                [2, 0]
            ];

            random.Shuffle(initDirs);

            int[][] dirs = RandomizeDirs(initDirs, lastDirs);

            int i = 0;
            foreach (int[] dir in dirs)
            {
                if (CanGo(x, y, h, w, dir, maze, true))
                {
                    maze
                    [y + dir[1] / 2]
                    [x + dir[0] / 2]
                    = false;

                    maze
                    [y + dir[1]]
                    [x + dir[0]]
                    = false;

                    GenerateMaze(h, w, x + dir[0], y + dir[1], maze, dir);
                }
                i++;
            }

            return maze;
        }
        public static int[][] RandomizeDirs(int[][] dirs, int[] lastDir)
        {
            Random random = new();
            List<int[]> newDirs = [];


            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i][0] != lastDir[0] || dirs[i][1] != lastDir[1])
                {
                    newDirs.Add(dirs[i]);
                }
            }
            newDirs.Insert(random.Next(0, 3) == 0 ? 0 : newDirs.Count, lastDir);

            return [.. newDirs];
        }
        public static bool CanGo(int x, int y, int h, int w, int[] dir, bool[][] maze, bool isGenerating)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];

            if (nx < 0 || nx >= w)
                return false;
            else if (ny < 0 || ny >= h)
                return false;
            else if (!isGenerating && maze[ny][nx] == true)
                return false;
            else if (isGenerating && maze[ny][nx] == false)
                return false;
            else if (isGenerating)
                return maze[ny][nx];
            else
                return !maze[ny][nx];
        }
    }
}