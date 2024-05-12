using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared;

public class RollingList<T> : IEnumerable<T>
{
    private readonly LinkedList<T> _list;

    public RollingList(int size)
    {
        _list = new LinkedList<T>();
        MaximumCount = size;
    }

    public int MaximumCount { get; }
    public int Count => _list.Count;

    public void Add(T item)
    {
        if (_list.Count == MaximumCount)
        {
            _list.RemoveFirst();
        }

        _list.AddLast(item);
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException();

            return _list.Skip(index).First();
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}
