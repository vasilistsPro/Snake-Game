using System;

class Program
{
    static Random random = new Random();
    static int height = Console.WindowHeight - 1;
    static int width = Console.WindowWidth - 5;
    static bool shouldExit = false;

    static int playerX = 0;
    static int playerY = 0;

    static int foodX = 0;
    static int foodY = 0;

    static string[] states = { "('-')", "(^-^)", "(X_X)" };
    static string[] foods = { "@@@@@", "$$$$$", "#####" };
    static string player = states[0];
    static int food = 0;

    static void Main()
    {
        InitializeGame();

        while (!shouldExit)
        {
            if (TerminalResized())
            {
                Console.Clear();
                Console.Write("Console was resized. Program exiting.");
                shouldExit = true;
            }
            else
            {
                if (PlayerIsFaster())
                {
                    Move(2, false); // Increased speed when player is faster
                }
                else if (PlayerIsSick())
                {
                    FreezePlayer();
                }
                else
                {
                    Move(); // Normal speed
                }
                if (GotFood())
                {
                    ChangePlayer();
                    ShowFood();
                }
            }
        }
    }

    static bool TerminalResized()
    {
        return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
    }

    static void ShowFood()
    {
        food = random.Next(0, foods.Length);
        foodX = random.Next(0, width - player.Length);
        foodY = random.Next(0, height - 1);

        Console.SetCursorPosition(foodX, foodY);
        Console.Write(foods[food]);
    }

    static bool GotFood()
    {
        return playerY == foodY && playerX == foodX;
    }

    static bool PlayerIsSick()
    {
        return player.Equals(states[2]);
    }

    static bool PlayerIsFaster()
    {
        return player.Equals(states[1]);
    }

    static void ChangePlayer()
    {
        player = states[food];
        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }

    static void FreezePlayer()
    {
        System.Threading.Thread.Sleep(1000);
        player = states[0];
    }

    static void Move(int speed = 1, bool otherKeysExit = false)
    {
        int lastX = playerX;
        int lastY = playerY;

        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                playerY--;
                break;
            case ConsoleKey.DownArrow:
                playerY++;
                break;
            case ConsoleKey.LeftArrow:
                playerX -= speed;
                break;
            case ConsoleKey.RightArrow:
                playerX += speed;
                break;
            case ConsoleKey.Escape:
                shouldExit = true;
                break;
            default:
                shouldExit = otherKeysExit;
                break;
        }

        Console.SetCursorPosition(lastX, lastY);
        for (int i = 0; i < player.Length; i++)
        {
            Console.Write(" ");
        }

        playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
        playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }

    static void InitializeGame()
    {
        Console.Clear();
        ShowFood();
        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }
}
