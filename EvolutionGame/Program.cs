namespace EvolutionGame;

class Program
{
    static void Main(string[] args)
    {
        HandleArgs(args);
        ShowIntro();
    }

    private static void ShowIntro()
    {
        Console.WriteLine("Hello, Welcome to Evolution Game!");
        Console.WriteLine("Hier zou een intro tekst kunnen komen te staan en andere zaken die leuk zijn om als eerste te tonen. Voor nu, alleen deze saaie regel tekst.");
    }

    /*  Onderstaande methode vangt eventuele commandline argumenten af, wanneer deze worden meegegeven.
     */
    public static void HandleArgs(string[] args)
    {
        if (args.Length == 0)
        {
            // Just nothing to do here right now.
        }
    }
}