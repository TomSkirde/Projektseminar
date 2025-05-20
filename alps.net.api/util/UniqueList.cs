using System.Collections.Generic;

public interface IUniqueList<T> : IList<T>
{
    // A list which can contain each item only once


    new bool Add(T item);

    new bool Insert(int index, T item);
}

public class UniqueList<T> : List<T>, IUniqueList<T>
{

    public new bool Add(T item)
    {
        if (!Contains(item))
        {
            base.Add(item);
            return true;
        }
        return false;
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        var uniqueEntries = getUniqueItems(collection);
        AddRange(uniqueEntries);
    }

    public new bool Insert(int index, T item)
    {
        if (!Contains(item))
        {
            base.Insert(index, item);
            return true;
        }
        return false;
    }
    public new void InsertRange(int index, IEnumerable<T> collection)
    {
        var uniqueEntries = getUniqueItems(collection);
        InsertRange(index, uniqueEntries);
    }

    IList<T> getUniqueItems(IEnumerable<T> items)
    {
        IList<T> uniqueEntries = new List<T>();
        foreach (T entry in items)
        {
            if (!Contains(entry))
                uniqueEntries.Add(entry);
        }
        return uniqueEntries;
    }
}