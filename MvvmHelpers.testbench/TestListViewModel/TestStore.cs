using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmHelpers.testbench.TestListViewModel;


public class TestStore
{

    public static async Task<IEnumerable<TestModel>> GetData(int itemCount)
    {
        var result = new List<TestModel>();
        for (int i = 0; i < itemCount; i++)
        {
            result.Add(new TestModel
            {
                Id = i,
                Text = $"This is item number {i}",
                Word = string.Join(",", Enumerable.Range(0, i).Select(_ => "Word")),
            });
        }
        await Task.Delay(1000);//simulate remote work
        return result;
    }
}