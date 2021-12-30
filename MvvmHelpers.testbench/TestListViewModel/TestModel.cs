namespace MvvmHelpers.testbench.TestListViewModel;

public class TestModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Word { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
