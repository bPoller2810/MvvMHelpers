using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmHelpers.test.Models;

internal class DummyObject
{
    public int Number { get; set; }
    public bool Show { get; set; }

    public DummyObject(int number, bool show)
    {
        Number = number;
        Show = show;
    }

}
