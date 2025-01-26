using Newtonsoft.Json;

namespace EvolutionGame;

public static class StorageHelper
{
    
    public static void SaveToFile(Player player)
    {
        string filePath = GetFilePath();
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        // Serialize the player
        string jsonData = JsonConvert.SerializeObject(player, Formatting.Indented);

        // Write data to file
        File.WriteAllText(filePath, jsonData);
    }

    public static Player? LoadFromFile()
    {
        string filePath = GetFilePath();

        if (!File.Exists(filePath))
        {
            return null; // return null if there is no file
            
        }

        // Read JSON data from file
        string jsonData = File.ReadAllText(filePath);

        // Deserialize JSON data to a Player-object
        return JsonConvert.DeserializeObject<Player>(jsonData);
    }
    
    private static string GetFilePath()
    {
        string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EvolutionGame"
        );

        return Path.Combine(folderPath, "player.json");
    }
}