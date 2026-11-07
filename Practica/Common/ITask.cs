namespace CSharpPracticum.Common;


public interface ITask
{
    string Title { get; }

    string Description { get; }

    string[] Requirements { get; }

    string Hint { get; }

    void Run();
}
