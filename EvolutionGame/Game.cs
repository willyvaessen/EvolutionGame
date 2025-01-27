using Newtonsoft.Json;

namespace EvolutionGame;

public class Game
{
    private bool _gameActive;
    private const int ScreenWidth = 78;
    private const int MaxElements = 64;

    [JsonProperty] private List<Element> _elements;

    /*  Constructor for this class.
     *  It contains the constructor for the Game Class.
     */
    public Game()
    {
        Element.InitializeElementList(); // Initialiseer de lijst met elementen
    }

    /*  End of Constructor for this class.
     *  This comment marks the end of the Constructor for this class.
     */

    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */


    public static Game? LoadGame()
    {
        return StorageHelper.LoadFromFile<Game>("game.json");
    }

    public void BuildGameScreen(Player player)
    {
        LoadOrInitializeGame();
        _gameActive = true;

        while (_gameActive)
        {
            Console.Clear();
            ShowTopBar(player);
            ShowGameContent();
            ShowMenu();
            GetMenuChoice(player);
        }
    }

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */

    private void LoadOrInitializeGame()
    {
        // Try loading an existing game and load the elements from that game file
        var loadedGame = StorageHelper.LoadFromFile<Game>("game.json");
        if (loadedGame != null)
        {
            Console.WriteLine("Loading saved game data...");
            _elements = loadedGame._elements; // Copy elements from loaded game
        }
        // or initialize a new list of elements
        else
        {
            Console.WriteLine("No saved game found. Starting a new game...");
            _elements = Enumerable.Repeat(Element.CreateEmptyElement(), MaxElements)
                .ToList(); // Fill grid with empty elements
        }
    }

    private void ShowTopBar(Player player)
    {
        DrawBox(ScreenWidth, new List<string> { player.ToString() });
    }

    private void ShowGameContent()
    {
        int boxHeight = 15;
        int elementsPerRow = 8; // We tonen 8 elementen per rij
        var visibleContent = new List<string>();
        string padding = "        "; // Witruimte links en rechts

        string currentRow = padding;

        for (int i = 0; i < _elements.Count; i++)
        {
            // Voeg element toe aan de rij
            currentRow += _elements[i].GetDisplayText(i + 1).PadRight(8);

            // Voeg rij toe na elk volledige set elementen
            if ((i + 1) % elementsPerRow == 0 || i == _elements.Count - 1)
            {
                visibleContent.Add(currentRow);
                visibleContent.Add(string.Empty); // Voeg een lege regel toe
                currentRow = padding; // Reset rij
            }
        }

        // Fills remaining lines until box height
        while (visibleContent.Count < boxHeight)
        {
            visibleContent.Add(string.Empty);
        }

        // Shows content in a box
        DrawBox(ScreenWidth, visibleContent);
    }

    private void ShowMenu()
    {
        DrawBox(ScreenWidth, new List<string>
        {
            "[1] Spawn Element | [2] Select Element | [3] View Elements | [X] Exit"
        });
    }

    private void GetMenuChoice(Player player)
    {
        while (true)
        {
            Console.Write(">> ");
            string menuChoice = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

            switch (menuChoice)
            {
                case "1":
                    SpawnElement(player);
                    Console.Clear();
                    ShowTopBar(player);
                    ShowGameContent(); // Grid will be drawn again with new elements
                    ShowMenu();
                    break;
                case "2":
                    Console.WriteLine("Option 2 chosen ...");
                    break;
                case "3":
                    Console.WriteLine("Option 3 chosen ...");
                    break;
                case "X":
                    Console.WriteLine("Exiting game...");
                    _gameActive = false;
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void SpawnElement(Player player)
    {
        // Find empty indexes of empty cells (type "XX")
        var emptyIndices = _elements
            .Select((element, index) => new { element, index })
            .Where(e => e.element.Type == "XX")
            .Select(e => e.index)
            .ToList();

        if (!emptyIndices.Any())
        {
            Console.WriteLine("No empty slots available to spawn an element.");
            return;
        }

        // Retrieve available elements based on players level
        var unlockedElements = Element.GetUnlockedElements(player.GetLevel());
        if (!unlockedElements.Any())
        {
            Console.WriteLine("No elements available to spawn for this level.");
            return;
        }

        //  Pick a random empty cell and a random available element.
        var random = new Random();
        int randomEmptyIndex = emptyIndices[random.Next(emptyIndices.Count)];
        Element randomElement = unlockedElements[random.Next(unlockedElements.Count)];

        // Place the element on the grid
        _elements[randomEmptyIndex] = randomElement;
        Save();
        Console.WriteLine($"Spawned element {randomElement.Type} at index {randomEmptyIndex}.");
    }

    private void DrawBox(int width, List<string> content)
    {
        string topBorder = $"┌{new string('─', width)}┐";
        string bottomBorder = $"└{new string('─', width)}┘";

        Console.WriteLine(topBorder);
        foreach (var line in content)
        {
            Console.WriteLine($"│{line.PadRight(width)}│");
        }

        Console.WriteLine(bottomBorder);
    }

    private void Save()
    {
        StorageHelper.SaveToFile(this);
    }
}