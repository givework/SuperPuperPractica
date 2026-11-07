namespace CSharpPracticum.Common;

public abstract class TaskBase : ITask
{

    protected TaskBase(string title, string description, string[] requirements, string hint)
    {
        Title = title;
        Description = description;
        Requirements = requirements;
        Hint = hint;
    }

    public string Title { get; }

    public string Description { get; }

    public string[] Requirements { get; }

    public string Hint { get; }

    public abstract void Run();
}
