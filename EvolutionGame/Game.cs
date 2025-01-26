namespace EvolutionGame;

public class Game
{
    private bool _gameActive;
    private const int ScreenWidth = 78;
    private const int MaxElements = 64;

    private readonly List<Element>
        _elements = Enumerable.Repeat(Element.CreateEmptyElement(), MaxElements).ToList(); // Dynamische grid met elementen

    /*  Constructor for this class.
     *  It contains the constructor for the Game Class.
     */

    /*  End of Constructor for this class.
     *  This comment marks the end of the Constructor for this class.
     */

    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */

    public void BuildGameScreen(Player player)
    {
        _gameActive = true;
        while (_gameActive)
        {
            Console.Clear();
            ShowTopBar(player);
            ShowGameContent();
            ShowMenu();
            GetMenuChoice();
        }
    }

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */
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
            "[1] Start Game | [2] View Organisms | [3] View Elements | [4] Exit"
        });
    }

    private void GetMenuChoice()
    {
        Console.Write(">> ");
        string? menuChoice = Console.ReadLine();
        switch (menuChoice)
        {
            case "1":
                Console.WriteLine("Option 1 chosen ...");
                break;
            case "2":
                Console.WriteLine("Option 2 chosen ...");
                break;
            case "3":
                Console.WriteLine("Option 3 chosen ...");
                break;
            case "4":
                Console.WriteLine("Option 4 chosen ...");
                _gameActive = false;
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    private void SpawnElement()
    {
        Console.WriteLine("SpawnElement is not implemented yet.");
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
}