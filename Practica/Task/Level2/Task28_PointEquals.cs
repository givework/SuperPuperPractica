using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public class Point : IEquatable<Point>
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public bool Equals(Point? other) => other is not null && X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => Equals(obj as Point);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";
}

public class Task28_PointEquals : TaskBase
{
    public Task28_PointEquals() : base(
        "Equals / GetHashCode для Point",
        "Переопределить Equals и GetHashCode для Point(x, y).",
        new[]
        {
            "Переопределить Equals(object) и GetHashCode()",
            "Реализовать IEquatable<Point>",
            "Точки с одинаковыми координатами равны",
            "HashSet<Point>: дубликат не увеличивает Count",
        },
        "IEquatable<T>, HashCode.Combine, HashSet<T>.")
    {
    }

    public override void Run()
    {
        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        var p3 = new Point(3, 4);

        Console.WriteLine($"p1.Equals(p2) = {p1.Equals(p2)} (ожидается True)");
        Console.WriteLine($"p1.Equals(p3) = {p1.Equals(p3)} (ожидается False)");
        Console.WriteLine($"p1.Equals((object)p2) = {p1.Equals((object)p2)}");

        var set = new HashSet<Point> { p1, p3 };
        bool added = set.Add(p2);
        Console.WriteLine($"Добавление дубликата (1, 2): {added}, Count = {set.Count} (ожидается 2)");
    }
}
