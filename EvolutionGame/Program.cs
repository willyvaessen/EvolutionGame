namespace EvolutionGame;

class Program
{
    /*  De Main methode is het toegangspunt van mijn C#-toepassing.
     *  Wanneer de toepassing wordt gestart, is de Main methode de eerste methode die wordt aangeroepen.
     */
    static void Main(string[] args)
    {
        HandleArgs(args);
        ShowIntro();

        // var player = new Player("TestPlayer");
        var player = LoadOrCreatePlayer();
        ShowMainMenu(player);
    }

    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */

    private static void ShowIntro()
    {
        Console.WriteLine("Hello, Welcome to Evolution Game!");
        Console.WriteLine(
            "Hier zou een intro tekst kunnen komen te staan en andere zaken die leuk zijn om als eerste te tonen. Voor nu, alleen deze saaie regel tekst.");
    }

    private static Player LoadOrCreatePlayer()
    {
        var existingPlayer = StorageHelper.LoadFromFile();
        if (existingPlayer != null)
        {
            Console.WriteLine($"Welcome back, {existingPlayer.GetName()}!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return existingPlayer;
        }

        string? name;
        do
        {
            Console.WriteLine("No profile found. Let's create one!");
            Console.Write("Enter your name:");
            name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Please try again.");
            }
        } while (string.IsNullOrWhiteSpace(name));

        var newPlayer = new Player(name);
        Console.WriteLine($"Welcome, {newPlayer.GetName()}!");

        StorageHelper.SaveToFile(newPlayer);
        Console.WriteLine("Press any key to continue...");
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
            Console.WriteLine("X. Exit Game");
            Console.Write("Enter your choice:");

            string menuChoice = (Console.ReadLine() ?? string.Empty).Trim().ToUpper(); // Zorg dat menuChoice nooit null is

            switch (menuChoice)
            {
                case "1":
                    Console.WriteLine("Starting/Resuming game...");
                    // Logica voor Start/Resume Game
                    StartGame(player);
                    break;
                case "2":
                    Console.WriteLine("Editing player info...");
                    // Logica voor Edit Player Info
                    break;
                case "X":
                    Console.WriteLine("Exiting game...");
                    running = false;
                    break;
#if DEBUG
                case "42": // Verborgen testoptie
                    ShowTestMenu(player);
                    break;
#endif
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

#if DEBUG
    private static void ShowTestMenu(Player player)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("=== Test Menu ===");
            Console.WriteLine("1. Add Experience");
            Console.WriteLine("2. Show Player Info");
            Console.WriteLine("3. Return to Main Menu");
            Console.Write("Enter your choice:");

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
        }
    }
#endif

    private static void StartGame(Player player)
    {
        Game game = new Game();
        game.BuildGameScreen(player);
    }

    /*  Onderstaande methode vangt eventuele commandline argumenten af wanneer deze worden meegegeven.
     */
    private static void HandleArgs(string[] args)
    {
        if (args.Length > 0)
        {
            Console.WriteLine("Command-line arguments detected: " + string.Join(", ", args));
            // Voeg eventuele specifieke verwerking hier toe.
        }
    }
}