using MvvmHelpers.testbench.TestListViewModel.ClassTest;
using MvvmHelpers.testbench.TestListViewModel.RecordTest;


//var collection = new ConditionalObservableCollection<string>(s => s.Length > 0);
//collection.Add("hi");
//collection.Add("");
//collection.Add("hio");
//collection.AddRange(new List<string> { "1", "2" });

//var count = collection.Count;
//var visible = collection.VisibleCount;




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
