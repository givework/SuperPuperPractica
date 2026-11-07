using System.Reflection;
using System.Text.RegularExpressions;
using CSharpPracticum.Common;

namespace CSharpPracticum.Menu;

public static class TaskRegistry
{
    private static readonly Regex NumberRegex = new(@"^Task(\d+)_", RegexOptions.Compiled);

    public static List<(int Number, ITask Task)> GetTasks(int from, int to)
    {
        var result = new List<(int Number, ITask Task)>();
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsAbstract || !typeof(ITask).IsAssignableFrom(type))
            {
                continue;
            }

            Match m = NumberRegex.Match(type.Name);
            if (!m.Success)
            {
                continue;
            }

            int number = int.Parse(m.Groups[1].Value);
            if (number >= from && number <= to && Activator.CreateInstance(type) is ITask task)
            {
                result.Add((number, task));
            }
        }

        result.Sort((a, b) => a.Number.CompareTo(b.Number));
        return result;
    }
}
