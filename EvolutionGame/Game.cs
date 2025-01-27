using Newtonsoft.Json;

namespace EvolutionGame;

public class Game
{
    private bool _gameActive;
    private const int ScreenWidth = 78;
    private const int MaxElements = 64;
    private int? _firstElementIndex;
    private int? _secondElementIndex;

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
        DrawBox("topBox", ScreenWidth, new List<string> { player.ToString() });
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
        DrawBox("middleBox", ScreenWidth, visibleContent);
    }

    private void ShowMenu()
    {
        DrawBox("bottomBox", ScreenWidth, new List<string>
        {
            "[1] Spawn Element | [2] Select Element | [3] View Elements | [X] Exit"
        });
    }

    private void ShowElementMenu(Player player, Element selectedElement)
    {
        DrawBox("bottomBox", ScreenWidth, new List<string>
        {
            "  [1] Merge Element    [2] Feed Element to Organism         [B] Back to Game"
        });
        Console.Write(">> ");
        string menuChoice = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

        switch (menuChoice)
        {
            case "1":
                SelectElementToMergewith(player, selectedElement);
                break;
            case "2":
                Console.WriteLine("Feed Element selected.");
                break;
            case "B":
                BuildGameScreen(player);
                break;
        }
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
                    SelectElement(player);
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

    private void SelectElement(Player player)
    {
        Console.Clear();
        ShowTopBar(player);
        ShowGameContent();
        int? selectedElement = GetSelectedElementFromUser();
        if (selectedElement == null) return; // Als Escape is ingedrukt, keer terug naar het spel

        ShowSingleElement(selectedElement.Value, player);
    }

    private void ShowSingleElement(int elementNumber, Player player)
    {
        int selectedElementIndex = elementNumber - 1;
        var selectedElement = _elements[selectedElementIndex];
        _firstElementIndex = selectedElementIndex;

        Console.Clear();
        ShowTopBar(player);
        Console.WriteLine($"You selected element {selectedElement.Type} at index {selectedElementIndex}.");
        ShowElementMenu(player, selectedElement);
    }

    private int? GetSelectedElementFromUser()
    {
        while (true)
        {
            DrawBox("bottomBox", ScreenWidth, new List<string>
            {
                "Enter Element Number (Nr between ()) or press Esc to go back: "
            });

            Console.Write(">> ");
            var keyInfo = Console.ReadKey(intercept: true);

            // Controleer op Escape-toets
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("\nReturning to the game...");
                return null; // Geeft null terug als de speler annuleert
            }

            // Controleer of de toets een cijfer is
            if (char.IsDigit(keyInfo.KeyChar))
            {
                Console.Write(keyInfo.KeyChar); // Laat het cijfer zien in de console
                string input = keyInfo.KeyChar.ToString();

                // Vraag de gebruiker om de rest van de invoer in te typen (indien meer dan één cijfer nodig is)
                string? additionalInput = Console.ReadLine()?.Trim();

                // Combineer de eerste toets met eventuele aanvullende invoer
                input += additionalInput;

                // Probeer de invoer naar een geldig nummer te converteren
                if (int.TryParse(input, out int selectedElementNumber) &&
                    selectedElementNumber >= 1 && selectedElementNumber <= MaxElements)
                {
                    Console.WriteLine($"\nYou selected element {selectedElementNumber}.");
                    return selectedElementNumber; // Retourneer het geselecteerde elementnummer
                }
            }

            Console.WriteLine("\nInvalid input. Please enter a valid element number or press Esc to go back.");
        }
    }

    private void SelectElementToMergewith(Player player, Element selectedElement)
    {
        var firstElement = selectedElement;
        Element secondElement = Element.CreateEmptyElement(); //  Create empty element to replace with user choice.
        Console.Clear();
        ShowTopBar(player);
        ShowGameContent();
        int? elementNumber = GetSelectedElementFromUser();

        if (elementNumber != null)
        {
            int selectedElementIndex = elementNumber.Value - 1;

            secondElement = _elements[selectedElementIndex];
            _secondElementIndex = selectedElementIndex;
            Console.WriteLine($"Element {firstElement.Type} selected to merge with element {secondElement.Type}");
        }
        else
        {
            Console.WriteLine("No element selected. Returning to game.");
            return; // Back to game without merging
        }

        if (_firstElementIndex.HasValue && _secondElementIndex.HasValue)
        {
            ShowMergeScreen(player, firstElement, secondElement, _firstElementIndex.Value, _secondElementIndex.Value);
        }
        else
        {
            Console.WriteLine("Element indices are not set properly.");
        }
        Console.ReadKey();
    }

    private void ShowMergeScreen(Player player, Element firstElement, Element secondElement, int firstElementIndex, int secondElementIndex)
    {
        Console.Clear();

        if (_firstElementIndex != null && _secondElementIndex != null)
        {
            // Toon bovenste info-bar
            DrawBox("topBox", ScreenWidth, new List<string>
            {
                $"Merging Elements: {firstElement.Type} and {secondElement.Type}"
            });

            // Toon de details van de elementen
            DrawBox("middleBox", ScreenWidth, new List<string>
            {
                $"Element 1: {firstElement.Type} - {firstElement.Description}",
                $"Element 2: {secondElement.Type} - {secondElement.Description}",
                " ",
                "[1] Confirm Merge | [B] Back to Game"
            });

            // Vraag om input
            Console.Write(">> ");
            string? choice = Console.ReadLine()?.Trim().ToUpper();

            if (choice == "1")
            {
                // Voer merge-logica uit
                var mergedElement = Element.Merge(firstElement, secondElement);
                _elements[firstElementIndex] = Element.CreateEmptyElement();
                _elements[secondElementIndex] = mergedElement;
                Save();
                Console.WriteLine("Merging elements...");
                BuildGameScreen(player);
            }
            else if (choice == "B")
            {
                // Ga terug naar het spel
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
                if (_firstElementIndex.HasValue && _secondElementIndex.HasValue)
                {
                    ShowMergeScreen(player, firstElement, secondElement, _firstElementIndex.Value, _secondElementIndex.Value);
                }
                else
                {
                    Console.WriteLine("Element indices are not set properly.");
                }
            }

        }
    }

    private void DrawBox(string boxType, int width, List<string> content)
    {
        string topBorder = $"\u2554{new string('\u2550', width)}\u2557"; // Bovenrand
        string divider = $"\u2560{new string('\u2550', width)}\u2563"; // Scheidingslijn
        string bottomBorder = $"\u255a{new string('\u2550', width)}\u255d"; // Onderkant

        // Controleer het type box en teken de juiste randen
        if (boxType == "topBox")
        {
            Console.WriteLine(topBorder); // Alleen de bovenrand
        }
        else if (boxType == "middleBox")
        {
            Console.WriteLine(divider); // Alleen de scheidingslijn
        }
        else if (boxType == "bottomBox")
        {
            Console.WriteLine(divider); // Begin met een scheidingslijn
        }

        // Toon de inhoud
        foreach (var line in content)
        {
            Console.WriteLine($"\u2551{line.PadRight(width)}\u2551"); // Binnenrand
        }

        // Sluit af met een rand, indien nodig
        if (boxType == "middleBox")
        {
            Console.WriteLine(divider); // Eindig met een scheidingslijn
        }
        else if (boxType == "bottomBox")
        {
            Console.WriteLine(bottomBorder); // Eindig met een onderrand
        }
    }

    private void Save()
    {
        StorageHelper.SaveToFile(this);
    }
}