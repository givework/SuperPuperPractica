using CSharpPracticum.Common;

namespace CSharpPracticum.Menu;

public class LevelMenu
{
    private readonly string _name;
    private readonly int _from;
    private readonly int _to;

    public LevelMenu(string name, int from, int to)
    {
        _name = name;
        _from = from;
        _to = to;
    }

    public void Show()
    {
        var tasks = TaskRegistry.GetTasks(_from, _to);
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {_name} ---");
            if (tasks.Count == 0)
            {
                Console.WriteLine("(задания этого уровня ещё не добавлены)");
            }

            foreach (var (number, task) in tasks)
            {
                Console.WriteLine($"{number}. {task.Title}");
            }

            Console.WriteLine("0. Назад");
            Console.WriteLine();
            string input = ConsoleHelper.ReadLine("Выберите задание (номер): ").Trim();

            if (input == "0" || input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (int.TryParse(input, out int n) && tasks.Exists(t => t.Number == n))
            {
                RunTask(n, tasks.Find(t => t.Number == n).Task);
            }
            else
            {
                Console.WriteLine("Нет такого задания.");
            }
        }
    }

    private static void RunTask(int number, ITask task)
    {
        Console.WriteLine();
        ConsoleHelper.Line('=');
        Console.WriteLine($"Задание {number}. {task.Title}");
        ConsoleHelper.Line();
        Console.WriteLine("Описание:");
        Console.WriteLine($"  {task.Description}");
        Console.WriteLine();
        Console.WriteLine("Требования:");
        foreach (string r in task.Requirements)
        {
            Console.WriteLine($"  - {r}");
        }

        Console.WriteLine();
        Console.WriteLine($"Подсказка: {task.Hint}");
        ConsoleHelper.Line();
        Console.WriteLine("[запуск решения...]");
        Console.WriteLine();

        try
        {
            task.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Необработанная ошибка в задании: {ex.GetType().Name}: {ex.Message}");
        }

        Console.WriteLine();
        Console.Write("Нажмите Enter для возврата в меню...");
        Console.ReadLine();
    }
}
