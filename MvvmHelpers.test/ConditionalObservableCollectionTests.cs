using MvvmHelpers.test.Models;
using MvvMHelpers.core;
using NUnit.Framework;

namespace MvvmHelpers.test;

[TestFixture]
public class Tests
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ConditionalObservableCollection<DummyObject> _collection;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [SetUp]
    public void Setup()
    {
        _collection = new ConditionalObservableCollection<DummyObject>(o => o.Show);
    }

    [TestCase]
    public void TestCollectionAdd()
    {
        var collectionRaised = 0;
        var propertyRaised = 0;
        _collection.PropertyChanged += (o, e) => propertyRaised++;
        _collection.CollectionChanged += (o, e) => collectionRaised++;
        _collection.Add(new DummyObject(1, true));

        Assert.AreEqual(1, collectionRaised);
        Assert.AreEqual(2, propertyRaised);
        Assert.AreEqual(1, _collection.Count);
        Assert.AreEqual(1, _collection.VisibleCount);
    }

    [TestCase]
    public void TestCollectionClear()
    {
        var collectionRaised = 0;
        var propertyRaised = 0;
        _collection.Add(new DummyObject(1, true));
        _collection.PropertyChanged += (o, e) => propertyRaised++;
        _collection.CollectionChanged += (o, e) => collectionRaised++;
        _collection.Clear();

        Assert.AreEqual(1, collectionRaised);
        Assert.AreEqual(2, propertyRaised);
        Assert.AreEqual(0, _collection.Count);
        Assert.AreEqual(0, _collection.VisibleCount);
    }
}