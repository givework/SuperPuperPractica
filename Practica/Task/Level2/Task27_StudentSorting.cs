using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public class Student : IComparable<Student>
{
    public Student(string name, double averageGrade)
    {
        Name = name;
        AverageGrade = averageGrade;
    }

    public string Name { get; }

    public double AverageGrade { get; }

    public int CompareTo(Student? other) =>
        other is null ? 1 : other.AverageGrade.CompareTo(AverageGrade);

    public override string ToString() => $"{Name} ({AverageGrade:F1})";
}

public class ByNameComparer : IComparer<Student>
{
    public int Compare(Student? x, Student? y) =>
        string.Compare(x?.Name, y?.Name, StringComparison.CurrentCultureIgnoreCase);
}

public class Task27_StudentSorting : TaskBase
{
    public Task27_StudentSorting() : base(
        "IComparable / IComparer<T> для Student",
        "Научить Student сортироваться двумя способами.",
        new[]
        {
            "Student: Name, AverageGrade",
            "IComparable<Student> — по среднему баллу (убывание)",
            "ByNameComparer : IComparer<Student> — по имени",
            "Показать обе сортировки на List<Student>",
        },
        "IComparable<T>, IComparer<T>, List<T>.Sort().")
    {
    }

    public override void Run()
    {
        var students = new List<Student>
        {
            new("Борис", 4.2), new("Анна", 4.9), new("Вера", 3.8), new("Георгий", 4.5),
        };

        students.Sort();
        Console.WriteLine("По среднему баллу (убывание): " + string.Join(", ", students));

        students.Sort(new ByNameComparer());
        Console.WriteLine("По имени:                      " + string.Join(", ", students));
    }
}
