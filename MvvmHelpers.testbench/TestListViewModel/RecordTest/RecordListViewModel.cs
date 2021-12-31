using MvvMHelpers.core;

namespace MvvmHelpers.testbench.TestListViewModel.RecordTest;

public class RecordListViewModel : BaseListViewModel<TestRecord, RecordItemViewModel>
{
    protected override Task<IEnumerable<TestRecord>> DataRequest => TestStore.GetRecordData(4);

    protected override object?[]? ItemArguments => null;

    protected override void ItemsLoaded(Exception? exception = null)
    {
        if (exception is null)
        {
            Console.WriteLine($"Loaded {Items.Count} Records");
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
