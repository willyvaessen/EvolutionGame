using Newtonsoft.Json;

namespace EvolutionGame;

/*  Primary Constructor for this class.
 *  It contains the constructor, as well as the private properties of the Player Class.
 */
public class Player(string name)
{
    [JsonProperty] private string Name { get; } = name; // Volledig private
    [JsonProperty] private int Level { get; set; } = 1; // Start op level 1
    [JsonProperty] private int Experience { get; set; } // Start met 0 XP

    /*  End of Primary Constructor for this class.
     *  This comment marks the end of the Primary Constructor for this class.
     */

    /*  Public methods.
     *  This section contains public methods that are available for other classes to work with.
     */

    public void Save()
    {
        StorageHelper.SaveToFile(this);
    }

    public string GetName()
    {
        return Name;
    }

    public int GetLevel()
    {
        return Level;
    }

    public void GainExperience(int points)
    {
        UpdateExperience(points);
    }

    public override string ToString()
    {
        return GetInfo();
    }

    /*  Private methods
     *  This section contains the private methods for this class, that will only be available internally.
     */
    private void UpdateLevel()
    {
        //  Logica om het level van de speler te verhogen.
        Level++;
        Save();
        Console.WriteLine($"Player {Name} has reached level {Level}");
    }

    public int GetExperience()
    {
        return Experience;
    }

    private void UpdateExperience(int points)
    {
        //  Logica om experience te verhogen
        Experience += points;
        Console.WriteLine($"Player {Name} now has {GetExperience()} experience.");
        while (Experience >= Level * 100)
        {
            UpdateLevel();
        }

        Save();
    }

    private string GetInfo()
    {
        return $"Name: {Name}, Level: {Level}, XP: {Experience}";
    }
}