using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonConsumerMetricsState
    {
        private const int HandlerDurationBucketCount = 10;

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
            long elapsedUs = (long)(elapsed.TotalMicroseconds);

            Interlocked.Increment(ref _handlersExecuted);
            Interlocked.Increment(ref _handlerDurationBuckets[GetHandlerDurationBucketIndex(elapsedUs)]);

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
            long elapsedUs = (long)(elapsed.TotalMicroseconds);
            Interlocked.Increment(ref _parseDurationBuckets[GetHandlerDurationBucketIndex(elapsedUs)]);
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

        private static int GetHandlerDurationBucketIndex(long microseconds)
        {
            if (microseconds < 100)
            {
                return 0; // <100us
            }

            if (microseconds < 1_000)
            {
                return 1; // 100us-1ms
            }

            if (microseconds < 10_000)
            {
                return 2; // 1-10ms
            }

            if (microseconds < 50_000)
            {
                return 3; // 10-50ms
            }

            if (microseconds < 250_000)
            {
                return 4; // 50-250ms
            }

            if (microseconds < 1_000_000)
            {
                return 5; // 250ms-1s
            }

            if (microseconds < 5_000_000)
            {
                return 6; // 1-5s
            }

            if (microseconds < 30_000_000)
            {
                return 7; // 5-30s
            }

            if (microseconds < 120_000_000)
            {
                return 8; // 30s-2m
            }

            return 9; // >=2m
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
