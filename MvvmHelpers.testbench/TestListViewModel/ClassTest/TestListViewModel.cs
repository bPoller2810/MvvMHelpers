using MvvMHelpers.core;

namespace MvvmHelpers.testbench.TestListViewModel.ClassTest;

public class TestListViewModel : BaseListViewModel<TestModel, TestItemViewModel>
{
    protected override Task<IEnumerable<TestModel>> DataRequest => TestStore.GetData(4);

    protected override object?[]? ItemArguments => null;

    protected override void ItemsLoaded(Exception? exception = null)
    {
        if (exception is null)
        {
            Console.WriteLine($"Loaded {Items.Count} Items");
            foreach (var item in Items)
            {
                Console.WriteLine($"-{item.Item.Id} {item.Item.Text}");
            }
        }
        else
        {
            Console.WriteLine(exception.ToString());
        }
    }
}
