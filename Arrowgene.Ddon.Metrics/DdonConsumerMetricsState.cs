using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Arrowgene.Networking.SAEAServer.Metric;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonConsumerMetricsState
    {
        private static readonly int HandlerDurationBucketCount = MetricBucketDefinitions.DurationBucketNames.Count;
        private static readonly long[] DurationBucketUpperBoundTicks = CreateDurationBucketUpperBoundTicks();

        private readonly long[] _handlerDurationBuckets = new long[HandlerDurationBucketCount];
        private readonly long[] _parseDurationBuckets = new long[HandlerDurationBucketCount];
        private long _handlersExecuted;
        private long _handlerErrors;
        private int _captureEnabled;

        private readonly ConcurrentDictionary<string, HandlerEntry> _handlerEntries = new();

        internal int HandlerDurationBucketsCount => HandlerDurationBucketCount;

        internal void EnableCapture()
        {
            Volatile.Write(ref _captureEnabled, 1);
        }

        internal void DisableCapture()
        {
            Volatile.Write(ref _captureEnabled, 0);
        }

        internal bool IsCaptureEnabled()
        {
            return Volatile.Read(ref _captureEnabled) == 1;
        }

        internal long GetHandlersExecuted()
        {
            return Volatile.Read(ref _handlersExecuted);
        }

        internal long GetHandlerErrors()
        {
            return Volatile.Read(ref _handlerErrors);
        }

        internal void CopyHandlerDurationBuckets(long[] destination)
        {
            CopyBuckets(_handlerDurationBuckets, destination);
        }

        internal void CopyParseDurationBuckets(long[] destination)
        {
            CopyBuckets(_parseDurationBuckets, destination);
        }

        private static void CopyBuckets(long[] source, long[] destination)
        {
            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.Length < source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(destination),
                    "Destination must be at least as large as the source counter array.");
            }

            for (int index = 0; index < source.Length; index++)
            {
                destination[index] = Volatile.Read(ref source[index]);
            }
        }

        internal ConcurrentDictionary<string, HandlerEntry> GetHandlerEntries()
        {
            return _handlerEntries;
        }

        internal void RecordHandlerExecution(
            string handlerId,
            string handlerName,
            long startTimestamp)
        {
            if (!IsCaptureEnabled())
            {
                return;
            }

            TimeSpan elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            Interlocked.Increment(ref _handlersExecuted);
            Interlocked.Increment(ref _handlerDurationBuckets[GetHandlerDurationBucketIndex(elapsed)]);

            HandlerEntry entry = _handlerEntries.GetOrAdd(
                handlerId, _ => new HandlerEntry(handlerName));
            entry.RecordExecution(elapsed.Ticks);
        }

        internal void RecordParseDuration(long receivedTimestamp)
        {
            if (!IsCaptureEnabled())
            {
                return;
            }

            TimeSpan elapsed = Stopwatch.GetElapsedTime(receivedTimestamp);
            Interlocked.Increment(ref _parseDurationBuckets[GetHandlerDurationBucketIndex(elapsed)]);
        }

        internal void IncrementHandlerErrors(string handlerId, string handlerName)
        {
            if (!IsCaptureEnabled())
            {
                return;
            }

            Interlocked.Increment(ref _handlerErrors);

            HandlerEntry entry = _handlerEntries.GetOrAdd(
                handlerId, _ => new HandlerEntry(handlerName));
            entry.IncrementErrors();
        }

        private static int GetHandlerDurationBucketIndex(TimeSpan elapsed)
        {
            long elapsedTicks = elapsed.Ticks;
            if (elapsedTicks < 0)
            {
                elapsedTicks = 0;
            }

            for (int index = 0; index < DurationBucketUpperBoundTicks.Length; index++)
            {
                if (elapsedTicks <= DurationBucketUpperBoundTicks[index])
                {
                    return index;
                }
            }

            return DurationBucketUpperBoundTicks.Length - 1;
        }

        private static long[] CreateDurationBucketUpperBoundTicks()
        {
            long[] bounds = new long[MetricBucketDefinitions.DurationBucketUpperBounds.Count];

            for (int index = 0; index < bounds.Length; index++)
            {
                bounds[index] = MetricBucketDefinitions.DurationBucketUpperBounds[index].Ticks;
            }

            return bounds;
        }

        internal sealed class HandlerEntry
        {
            private readonly string _handlerName;
            private long _executionCount;
            private long _errorCount;
            private long _totalDurationTicks;
            private long _minDurationTicks;
            private long _maxDurationTicks;

            public HandlerEntry(string handlerName)
            {
                _handlerName = handlerName ?? string.Empty;
                _minDurationTicks = long.MaxValue;
                _maxDurationTicks = long.MinValue;
            }

            internal string HandlerName => _handlerName;

            internal long GetExecutionCount()
            {
                return Volatile.Read(ref _executionCount);
            }

            internal long GetErrorCount()
            {
                return Volatile.Read(ref _errorCount);
            }

            internal long GetTotalDurationTicks()
            {
                return Volatile.Read(ref _totalDurationTicks);
            }

            internal long GetMinDurationTicks()
            {
                return Volatile.Read(ref _minDurationTicks);
            }

            internal long GetMaxDurationTicks()
            {
                return Volatile.Read(ref _maxDurationTicks);
            }

            public void RecordExecution(long durationTicks)
            {
                Interlocked.Increment(ref _executionCount);
                Interlocked.Add(ref _totalDurationTicks, durationTicks);
                UpdateMin(durationTicks);
                UpdateMax(durationTicks);
            }

            public void IncrementErrors()
            {
                Interlocked.Increment(ref _errorCount);
            }

            private void UpdateMin(long ticks)
            {
                long current = Volatile.Read(ref _minDurationTicks);
                while (ticks < current)
                {
                    long prev = Interlocked.CompareExchange(ref _minDurationTicks, ticks, current);
                    if (prev == current) break;
                    current = prev;
                }
            }

            private void UpdateMax(long ticks)
            {
                long current = Volatile.Read(ref _maxDurationTicks);
                while (ticks > current)
                {
                    long prev = Interlocked.CompareExchange(ref _maxDurationTicks, ticks, current);
                    if (prev == current) break;
                    current = prev;
                }
            }
        }
    }
}
