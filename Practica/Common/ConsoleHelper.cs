using System.Globalization;

namespace CSharpPracticum.Common;


public static class ConsoleHelper
{

    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

 
    public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            string s = ReadLine(prompt);
            if (int.TryParse(s, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine("Некорректный ввод, попробуйте ещё раз.");
        }
    }


    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            string s = ReadLine(prompt).Replace(',', '.');
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            Console.WriteLine("Некорректный ввод, попробуйте ещё раз.");
        }
    }

 
    public static void Line(char c = '-', int length = 50) => Console.WriteLine(new string(c, length));
}
