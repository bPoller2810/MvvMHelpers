using MvvMHelpers.core;


var collection = new ConditionalObservableCollection<string>(s => s.Length > 0);
collection.Add("hi");
collection.Add("");
collection.Add("hio");
collection.AddRange(new List<string> { "1", "2" });

var count = collection.Count;
var visible = collection.VisibleCount;


Console.WriteLine("stop");