using Newtonsoft.Json;

namespace EvolutionGame;

public static class StorageHelper
{
    public static void SaveToFile(Object dataToSave)
    {
        string fileName = dataToSave switch
        {
            Player => "player.json",
            Game => "game.json",
            _ => throw new InvalidOperationException("Unsupported data type")
        };
        string filePath = GetFilePath(fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        // Serialize the player
        string jsonData = JsonConvert.SerializeObject(dataToSave, Formatting.Indented);

        // Write data to file
        File.WriteAllText(filePath, jsonData);
    }

    public static T? LoadFromFile<T>(string fileName)
    {
        string filePath = GetFilePath(fileName);

        if (!File.Exists(filePath))
        {
            return default; // return null if there is no file
        }

        // Read JSON data from file
        string jsonData = File.ReadAllText(filePath);

        // Deserialize JSON data to a Player-object
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    private static string GetFilePath(string fileName)
    {
        string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EvolutionGame"
        );

        return Path.Combine(folderPath, fileName);
    }
}