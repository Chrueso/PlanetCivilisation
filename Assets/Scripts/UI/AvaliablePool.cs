using System;
using System.Collections.Generic;

/// <summary>
/// Generic fast-removal pool using swap-and-pop.
/// </summary>
public class AvailablePool<T> where T : class
{
    private readonly List<T> items;
    private readonly Dictionary<T, int> lookup;

    public int Count => items.Count;
    public bool IsEmpty => items.Count == 0;

    public AvailablePool()
    {
        items = new List<T>();
        lookup = new Dictionary<T, int>();
    }

    public AvailablePool(IEnumerable<T> source) : this()
    {
        AddRange(source);
    }

    public T GetRandom(System.Random rng)
    {
        if (items.Count == 0)
            return null;

        int randomIndex = rng.Next(items.Count);
        return items[randomIndex];
    }

    public void Add(T item)
    {
        if (lookup.ContainsKey(item))
            return;

        lookup[item] = items.Count;
        items.Add(item);
    }

    public void AddRange(IEnumerable<T> source)
    {
        foreach (var item in source)
            Add(item);
    }

    public void Remove(T item)
    {
        if (!lookup.TryGetValue(item, out int removeIndex))
            return;

        int lastIndex = items.Count - 1;
        T lastItem = items[lastIndex];

        // Move last item into removed slot
        items[removeIndex] = lastItem;
        lookup[lastItem] = removeIndex;

        // Remove last
        items.RemoveAt(lastIndex);
        lookup.Remove(item);
    }

    public void Remove(IEnumerable<T> toRemove)
    {
        foreach (var item in toRemove)
            Remove(item);
    }
}