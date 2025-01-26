namespace EvolutionGame;

class Program
{
    static void Main(string[] args)
    {
        HandleArgs(args);
        ShowIntro();
        
        // var player = new Player("TestPlayer");
        var player = LoadOrCreatePlayer();
        ShowMainMenu(player);

    }


    private static void ShowIntro()
    {
        Console.WriteLine("Hello, Welcome to Evolution Game!");
        Console.WriteLine("Hier zou een intro tekst kunnen komen te staan en andere zaken die leuk zijn om als eerste te tonen. Voor nu, alleen deze saaie regel tekst.");
    }
    
    private static Player LoadOrCreatePlayer()
    {
        var existingPlayer = StorageHelper.LoadFromFile();
        if (existingPlayer != null)
        {
            Console.WriteLine($"Welcome back, {existingPlayer.GetName()}!");
            Console.ReadKey();
            return existingPlayer;
        }

        Console.WriteLine("No profile found. Let's create one!");
        Console.WriteLine("Enter your name:");
        string? name = Console.ReadLine()?.Trim();

        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty. Please enter a valid name:");
            name = Console.ReadLine()?.Trim();
            Console.ReadKey();
        }

        var newPlayer = new Player(name);
        Console.WriteLine($"Welcome, {newPlayer.GetName()}!");

        StorageHelper.SaveToFile(newPlayer);
        Console.ReadKey();
        return newPlayer;
    }
    
    private static void ShowMainMenu(Player player)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Start/Resume Game");
            Console.WriteLine("2. Edit Player Info");
            Console.WriteLine("3. Exit Game");
            Console.WriteLine("Enter your choice:");

            string? choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Starting/Resuming game...");
                    // Logica voor Start/Resume Game
                    break;
                case "2":
                    Console.WriteLine("Editing player info...");
                    // Logica voor Edit Player Info
                    break;
                case "3":
                    Console.WriteLine("Exiting game...");
                    running = false;
                    break;
                case "42": // Verborgen testoptie
                    ShowTestMenu(player);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
            }
        }
    }
    
    private static void ShowTestMenu(Player player)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Test Menu ===");
            Console.WriteLine("1. Add Experience");
            Console.WriteLine("2. Show Player Info");
            Console.WriteLine("3. Return to Main Menu");
            Console.WriteLine("Enter your choice:");

            string? choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter XP to add:");
                    if (int.TryParse(Console.ReadLine(), out int xp))
                    {
                        player.GainExperience(xp);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    break;
                case "2":
                    Console.WriteLine(player);
                    break;
                case "3":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }


    /*  Onderstaande methode vangt eventuele commandline argumenten af, wanneer deze worden meegegeven.
     */
    private static void HandleArgs(string[] args)
    {
        if (args.Length == 0)
        {
            // Just nothing to do here right now.
        }
    }
}