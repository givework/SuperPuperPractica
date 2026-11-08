using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public interface IShape
{
    double Area();

    double Perimeter();
}

public class Circle : IShape
{
    private readonly double _r;

    public Circle(double r) => _r = r;

    public double Area() => Math.PI * _r * _r;

    public double Perimeter() => 2 * Math.PI * _r;

    public override string ToString() => $"Circle(r={_r})";
}

public class Rectangle : IShape
{
    private readonly double _w;
    private readonly double _h;

    public Rectangle(double w, double h)
    {
        _w = w;
        _h = h;
    }

    public double Area() => _w * _h;
    public double Perimeter() => 2 * (_w + _h);
    public override string ToString() => $"Rectangle({_w}x{_h})";
}

public class Triangle : IShape
{
    private readonly double _a;
    private readonly double _b;
    private readonly double _c;

    public Triangle(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public double Area()
    {
        double p = Perimeter() / 2;
        return Math.Sqrt(p * (p - _a) * (p - _b) * (p - _c));
    }

    public double Perimeter() => _a + _b + _c;
    public override string ToString() => $"Triangle({_a}, {_b}, {_c})";
}

public class Task25_Shapes : TaskBase
{
    public Task25_Shapes() : base(
        "Интерфейс IShape + сортировка фигур",
        "Реализовать интерфейс фигур и отсортировать их по площади.",
        new[]
        {
            "IShape: Area(), Perimeter()",
            "Circle, Rectangle, Triangle",
            "List<IShape> из 5+ фигур",
            "Сортировка по площади (List.Sort с лямбдой или OrderBy)",
            "Вывод до и после сортировки",
        },
        "interface, List<T>.Sort(Comparison<T>), формула Герона.")
    {
    }

    public override void Run()
    {
        var shapes = new List<IShape>
        {
            new Circle(3),
            new Rectangle(4, 5),
            new Triangle(3, 4, 5),
            new Circle(1),
            new Rectangle(2, 2),
            new Triangle(6, 7, 8),
        };

        Console.WriteLine("До сортировки:");
        Print(shapes);

        shapes.Sort((x, y) => x.Area().CompareTo(y.Area()));

        Console.WriteLine("После сортировки по площади:");
        Print(shapes);
    }

    private static void Print(List<IShape> shapes)
    {
        foreach (IShape s in shapes)
        {
            Console.WriteLine($"  {s,-22} S = {s.Area():F2}, P = {s.Perimeter():F2}");
        }
    }
}
