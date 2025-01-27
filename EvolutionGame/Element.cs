namespace EvolutionGame;

public class Element
{
    private const string EmptyType = "XX";
    public string Type { get; }
    public string Description { get; }
    private int UnlockLevel { get; }
    private static readonly Dictionary<string, Element> ElementList = new();

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

/*  
    <summary>
    Initializes the element list with default elements.
    This must be called before accessing the list.
    </summary>
*/
    public static void InitializeElementList()
    {
        ElementList.Clear();

        ElementList.Add("RI", new Element("RI", "Red Element", 1));
        ElementList.Add("GI", new Element("GI", "Green Element", 2));
        ElementList.Add("BI", new Element("BI", "Blue Element", 3));
        ElementList.Add("BL1", new Element("BL1", "Black Element Level 1", 4));
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
}