using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;

public class Task01_HelloWorld : TaskBase
{
  
    public Task01_HelloWorld() : base(
        "Hello, World + текущая дата",
        "Вывести приветствие и текущую дату.",
        new[]
        {
            "Использовать Console.WriteLine",
            "Дату получить через DateTime.Now, формат dd.MM.yyyy",
            "Вывести две строки: приветствие и дату",
        },
        "Console.WriteLine, DateTime.Now, ToString(\"dd.MM.yyyy\") (пространство имён System).")
    {
    }

    public override void Run()
    {
        Console.WriteLine("Поверка для 15 задания");
        Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy"));
    }
}
