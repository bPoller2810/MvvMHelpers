using MvvMHelpers.core;

namespace MvvmHelpers.testbench.TestListViewModel.ClassTest;


[GenerateItemProperties(new string[] { nameof(TestModel.Id)})]
public partial class TestItemViewModel : BaseItemViewModel<TestModel>
{
}


