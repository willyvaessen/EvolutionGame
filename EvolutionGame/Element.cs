namespace EvolutionGame;

/*  Primary Constructor for this class.
 *  It contains the constructor, as well as the private properties of the Element Class.
 */
public class Element(string type)
{
    private const string EmptyType = "XX";
    private string Type { get; } = type;

    /*  End of Primary Constructor for this class.
     *  This comment marks the end of the Primary Constructor for this class.
     */
    
    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */
    
    public string GetDisplayText(int index)
    {
        return Type == EmptyType ? string.Empty : $"({index}){Type}";
    }
}