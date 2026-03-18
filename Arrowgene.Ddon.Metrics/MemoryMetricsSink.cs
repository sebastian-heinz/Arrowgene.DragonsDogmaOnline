using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Arrowgene.Networking.Metrics;

namespace Arrowgene.Ddon.Metrics;

public sealed class MemoryMetricsSink : IMetricsSink<DdonServerMetricsSnapshot>
{
    private readonly Lock _lock = new();
    private readonly LinkedList<DdonServerMetricsSnapshot> _samples = new();
    private readonly TimeSpan _retention;

    public MemoryMetricsSink(TimeSpan retention)
    {
        _retention = retention;
    }

    public void Write(DdonServerMetricsSnapshot snapshot)
    {
        lock (_lock)
        {
            _samples.AddLast(snapshot);
            Evict(snapshot.TimestampUtc);
        }
    }

    public List<DdonServerMetricsSnapshot> GetSamples()
    {
        lock (_lock)
        {
            Evict(DateTime.UtcNow);
            return _samples.ToList();
        }
    }

    public DdonServerMetricsSnapshot? GetLatest()
    {
        lock (_lock)
        {
            return _samples.Last != null ? _samples.Last.Value : null;
        }
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _samples.Count;
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _samples.Clear();
        }
    }

    public void Dispose()
    {
        Clear();
    }

    private void Evict(DateTime now)
    {
        DateTime cutoff = now - _retention;
        while (_samples.First != null && _samples.First.Value.TimestampUtc < cutoff)
        {
            _samples.RemoveFirst();
        }
    }
}
