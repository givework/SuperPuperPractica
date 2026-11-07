namespace CSharpPracticum.Menu;


public static class MenuRunner
{

    public static void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        var levels = new[]
        {
            new LevelMenu("Уровень 1: Базовый", 1, 15),
            new LevelMenu("Уровень 2: Средний", 16, 35),
            new LevelMenu("Уровень 3: Продвинутый", 36, 50),
        };

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Практикум по C# ===");
            Console.WriteLine("1. Уровень 1 — Базовый (задания 1–15)");
            Console.WriteLine("2. Уровень 2 — Средний (задания 16–35)");
            Console.WriteLine("3. Уровень 3 — Продвинутый (задания 36–50)");
            Console.WriteLine("4. Выход");
            Console.WriteLine();
            Console.Write("Выберите уровень: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (input == "4" || input == "0" || input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (int.TryParse(input, out int n) && n >= 1 && n <= 3)
            {
                levels[n - 1].Show();
            }
            else
            {
                Console.WriteLine("Нет такого пункта.");
            }
        }
    }
}
