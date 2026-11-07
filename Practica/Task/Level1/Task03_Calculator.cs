using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task03_Calculator : TaskBase
{

    public Task03_Calculator() : base(
        "Калькулятор с делением на ноль",
        "Принимает два числа и оператор (+, -, *, /), выводит результат.",
        new[]
        {
            "Оператор обрабатывать через switch",
            "Деление на ноль — сообщение об ошибке, без падения",
            "Некорректный оператор — «Неизвестная операция»",
            "Ввод чисел — через double.TryParse",
        },
        "switch, double.TryParse, Console.ReadLine.")
    {
    }

 
    public override void Run()
    {
        double a = ConsoleHelper.ReadDouble("Первое число: ");
        string op = ConsoleHelper.ReadLine("Оператор (+ - * /): ").Trim();
        double b = ConsoleHelper.ReadDouble("Второе число: ");

        Console.WriteLine(Calculate(a, op, b));
    }

 
    public static string Calculate(double a, string op, double b)
    {
        switch (op)
        {
            case "+":
                return $"{a} + {b} = {a + b}";
            case "-":
                return $"{a} - {b} = {a - b}";
            case "*":
                return $"{a} * {b} = {a * b}";
            case "/":
                return b == 0 ? "Ошибка: деление на ноль!" : $"{a} / {b} = {a / b}";
            default:
                return "Неизвестная операция";
        }
    }
}
