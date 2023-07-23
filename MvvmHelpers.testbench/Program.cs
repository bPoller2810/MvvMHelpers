using MvvmHelpers.testbench.TestListViewModel.ClassTest;
using MvvmHelpers.testbench.TestListViewModel.RecordTest;
using MvvMHelpers.core;

var list = new TestListViewModel();
await list.LoadItems();
var firstItem = list.Items.First();
firstItem.PropertyChanged += (o, e) => Console.WriteLine($"{e.PropertyName} has been changed");
firstItem.Word = "newWord";
firstItem.Text = null;

var listRecord = new RecordListViewModel();
await listRecord.LoadItems();
var firstItemRecord = listRecord.Items.First();
firstItemRecord.PropertyChanged += (o, e) => Console.WriteLine($"{e.PropertyName} has been changed");
firstItemRecord.Word = "newWord";
firstItemRecord.Text = null;

Console.WriteLine("stop");

public class TestViewModel : BaseViewModel
{

    private string? _test;
    public string? Test
    {
        get => _test;
        set => Set(ref _test, value, () => Console.WriteLine("affe"));
    }

}