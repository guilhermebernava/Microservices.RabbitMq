namespace Publisher.Utils;

public static class ConsoleUtils
{
    public static void ShowErrorMessage(string errorMessage)
    {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {errorMessage}");
        Console.ForegroundColor = originalColor;
    }
}
