namespace EvolutionGame;

public class Element
{
    private const string EmptyType = "XX";
    public string Type { get; }
    public string Description { get; }
    private int UnlockLevel { get; }
    private static readonly Dictionary<string, Element> ElementList = new();

    private static readonly Dictionary<(string, string), string> MergeRules = new()
    {
        { ("RI", "RI"), "RII" }, // Red Level 1 + Red Level 1 = Red Level 2
        { ("RII", "RII"), "RIII" }, // Red Level 2 + Red Level 2 = Red Level 3
        { ("RIII", "RIII"), "RIV" }, // Red Level 3 + Red Level 3 = Red Level 4
        { ("GI", "GI"), "GII" }, // Green Level 1 + Green Level 1 = Green Level 2
        { ("GII", "GII"), "GIII" }, // Green Level 2 + Green Level 2 = Green Level 3
        { ("GIII", "GIII"), "GIV" }, // Green Level 3 + Green Level 3 = Green Level 4
        { ("BI", "BI"), "BII" }, // Blue Level 1 + Blue Level 1 = Blue Level 2
        { ("BII", "BII"), "BIII" }, // Blue Level 2 + Blue Level 2 = Blue Level 3
        { ("BIII", "BIII"), "BIV" }, // Blue Level 3 + Blue Level 3 = Blue Level 4
    };

    /*  Constructor for this class.
     *  It contains the constructor for the Element Class.
     */
    public Element(string type, string description, int unlockLevel)
    {
        Type = type;
        Description = description;
        UnlockLevel = unlockLevel;
    }

    /*  End of Constructor for this class.
     *  This comment marks the end of the Constructor for this class.
     */

    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */

    public static Element Merge(Element firstElement, Element secondElement)
    {
        Element mergedElement = PerformMerge(firstElement, secondElement);
        //  Logica om de merge te voltooien
        return mergedElement;
    }
    

/*
    <summary>
    Initializes the element list with default elements.
    This must be called before accessing the list.
    </summary>
*/
    public static void InitializeElementList()
    {
        ElementList.Clear();

        ElementList.Add("RI", new Element("RI", "Red Element Level 1", 1));
        ElementList.Add("RII", new Element("RII", "Red Element Level 2", 1));
        ElementList.Add("RIII", new Element("RIII", "Red Element Level 3", 2));
        ElementList.Add("RIV", new Element("RIV", "Red Element Level 4", 4));
        ElementList.Add("GI", new Element("GI", "Green Element Level 1", 2));
        ElementList.Add("GII", new Element("GII", "Green Element Level 2", 2));
        ElementList.Add("GIII", new Element("GIII", "Green Element Level 3", 3));
        ElementList.Add("GIV", new Element("GIV", "Green Element Level 4", 4));
        ElementList.Add("BI", new Element("BI", "Blue Element Level 1", 3));
        ElementList.Add("BII", new Element("BII", "Blue Element Level 2", 3));
        ElementList.Add("BIII", new Element("BIII", "Blue Element Level 3", 4));
        ElementList.Add("BIV", new Element("BIV", "Blue Element Level 4", 5));
        ElementList.Add("BL1", new Element("BL1", "Black Element Level 1", 5));
        ElementList.Add("DR", new Element("DR", "Dark Red Element", 6));
        ElementList.Add("DG", new Element("DG", "Dark Green Element", 6));
        ElementList.Add("DB", new Element("DB", "Dark Blue Element", 6));
    }

    public static Element CreateEmptyElement()
    {
        return new Element("XX", "Empty Slot", 0);
    }

    public static Element? GetElement(string type)
    {
        ElementList.TryGetValue(type, out var element);
        return element;
    }

    public static List<Element> GetUnlockedElements(int level)
    {
        return ElementList.Values.Where(e => e.UnlockLevel <= level).ToList();
    }

    public string GetDisplayText(int index)
    {
        return Type == EmptyType ? string.Empty : $"({index}){Type}";
    }

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */

    // Private helper method to generate a key-pair, used to compare to the Dictionary MergeRules
    private static (string, string) GetMergeKey(string type1, string type2)
    {
        return string.Compare(type1, type2, StringComparison.Ordinal) <= 0 
            ? (type1, type2) 
            : (type2, type1);
    }
    
    private static Element PerformMerge(Element firstElement, Element secondElement)
    {
        // Combine both types in a tuple, sorted alfabetically
        var key = GetMergeKey(firstElement.Type, secondElement.Type);

        // Check if combination exists in MergeRules 
        if (MergeRules.TryGetValue(key, out string? resultType))
        {
            // If the combination exists, return the resulting element
            return GetElement(resultType) ?? CreateEmptyElement(); // or return an empty element, in case something goes wrong
        }

        // If the combination doest not exist, return empty element
        return CreateEmptyElement();
    }
    
    private void Merge()
    {
        Console.WriteLine("This is the method that merges 2 elements.");
    }
}