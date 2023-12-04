namespace ProjectForAkelon.ExcelTask.Models;

public abstract class BaseClass
{
    protected BaseClass(int id)
    {
        Id = id;
    }

    public int Id { get; }
}