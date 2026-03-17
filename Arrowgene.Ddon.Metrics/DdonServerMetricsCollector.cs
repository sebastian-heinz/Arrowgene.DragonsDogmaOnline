using System;
using System.Collections.Generic;
using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonServerMetricsCollector : IDisposable
    {
        private const int SamplingIntervalMs = 1000;
        private const int ThreadJoinTimeoutMs = 10000;

        private readonly object _lifecycleSync;
        private readonly object _snapshotSync;
        private readonly DdonServerMetricsState _state;
        private CancellationTokenSource _cancellationTokenSource;
        private Thread _thread;
        private DdonServerMetricsSnapshot _latestSnapshot;
        private DateTime _serverStartedAtUtc;
        private DateTime _previousTimestampUtc;
        private long _previousHandlersExecuted;
        private long _previousHandlerErrors;
        private long _snapshotSequenceNumber;
        private bool _disposed;

        internal DdonServerMetricsCollector(DdonServerMetricsState state)
        {
            _lifecycleSync = new object();
            _snapshotSync = new object();
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _serverStartedAtUtc = DateTime.MinValue;
            _snapshotSequenceNumber = 0;
            _latestSnapshot = CreateSnapshot(DateTime.UtcNow, 0.0d, 0.0d);
            _previousTimestampUtc = _latestSnapshot.TimestampUtc;
        }

        internal void Start(string threadName)
        {
            Thread metricsThread;

            lock (_lifecycleSync)
            {
                ThrowIfDisposed();

                if (_thread is not null)
                {
                    return;
                }

                DateTime now = DateTime.UtcNow;
                _serverStartedAtUtc = now;
                _snapshotSequenceNumber = 0;
                _previousTimestampUtc = now;
                _previousHandlersExecuted = _state.ConsumerMetricsState.GetHandlersExecuted();
                _previousHandlerErrors = _state.ConsumerMetricsState.GetHandlerErrors();
                PublishSnapshotNoLock(now, 0.0d, 0.0d);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                metricsThread = new Thread(() => Run(cancellationTokenSource.Token))
                {
                    Name = threadName,
                    IsBackground = true
                };

                _cancellationTokenSource = cancellationTokenSource;
                _thread = metricsThread;
            }

            metricsThread.Start();
        }

        internal void Stop()
        {
            Thread metricsThread;
            CancellationTokenSource cancellationTokenSource;

            lock (_lifecycleSync)
            {
                metricsThread = _thread;
                cancellationTokenSource = _cancellationTokenSource;
                _thread = null;
                _cancellationTokenSource = null;
            }

            cancellationTokenSource?.Cancel();
            if (metricsThread != null && !metricsThread.Join(ThreadJoinTimeoutMs))
            {
                // Thread did not exit within timeout
            }
            cancellationTokenSource?.Dispose();
        }

        internal void CaptureSnapshot()
        {
            lock (_lifecycleSync)
            {
                ThrowIfDisposed();

                DateTime now = DateTime.UtcNow;
                long handlersExecuted = _state.ConsumerMetricsState.GetHandlersExecuted();
                long handlerErrors = _state.ConsumerMetricsState.GetHandlerErrors();
                double elapsedSeconds = (now - _previousTimestampUtc).TotalSeconds;

                double handlersExecutedPerSecond = 0.0d;
                double handlerErrorsPerSecond = 0.0d;

                if (elapsedSeconds > 0.0d)
                {
                    handlersExecutedPerSecond = (handlersExecuted - _previousHandlersExecuted) / elapsedSeconds;
                    handlerErrorsPerSecond = (handlerErrors - _previousHandlerErrors) / elapsedSeconds;
                }

                PublishSnapshotNoLock(now, handlersExecutedPerSecond, handlerErrorsPerSecond);
                _previousTimestampUtc = now;
                _previousHandlersExecuted = handlersExecuted;
                _previousHandlerErrors = handlerErrors;
            }
        }

        internal DdonServerMetricsSnapshot GetSnapshot()
        {
            lock (_snapshotSync)
            {
                return _latestSnapshot;
            }
        }

        public void Dispose()
        {
            lock (_lifecycleSync)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
            }

            Stop();
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (cancellationToken.WaitHandle.WaitOne(SamplingIntervalMs))
                {
                    return;
                }

                try
                {
                    CaptureSnapshot();
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
        }

        private DdonServerMetricsSnapshot CreateSnapshot(
            DateTime timestampUtc,
            double handlersExecutedPerSecond,
            double handlerErrorsPerSecond)
        {
            ConsumerMetricsSnapshot consumerSnapshot = CaptureConsumerSnapshot(_state.ConsumerMetricsState);
            long sequenceNumber = _serverStartedAtUtc == DateTime.MinValue
                ? 0
                : Interlocked.Increment(ref _snapshotSequenceNumber);

            return new DdonServerMetricsSnapshot(
                timestampUtc,
                _serverStartedAtUtc,
                sequenceNumber,
                handlersExecutedPerSecond,
                handlerErrorsPerSecond,
                consumerSnapshot
            );
        }

        private void PublishSnapshotNoLock(
            DateTime timestampUtc,
            double handlersExecutedPerSecond,
            double handlerErrorsPerSecond)
        {
            DdonServerMetricsSnapshot snapshot = CreateSnapshot(
                timestampUtc,
                handlersExecutedPerSecond,
                handlerErrorsPerSecond
            );

            lock (_snapshotSync)
            {
                _latestSnapshot = snapshot;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DdonServerMetricsCollector));
            }
        }

        private static ConsumerMetricsSnapshot CaptureConsumerSnapshot(ConsumerMetricsState state)
        {
            long[] durationBuckets = new long[state.HandlerDurationBucketsCount];
            state.CopyHandlerDurationBuckets(durationBuckets);

            Dictionary<string, ConsumerMetricsSnapshot.HandlerMetrics> handlerSnapshots = new();
            foreach (KeyValuePair<string, ConsumerMetricsState.HandlerEntry> kvp in state.GetHandlerEntries())
            {
                handlerSnapshots[kvp.Key] = CaptureHandlerSnapshot(kvp.Value);
            }

            return new ConsumerMetricsSnapshot(
                state.GetHandlersExecuted(),
                state.GetHandlerErrors(),
                durationBuckets,
                handlerSnapshots
            );
        }

        private static ConsumerMetricsSnapshot.HandlerMetrics CaptureHandlerSnapshot(ConsumerMetricsState.HandlerEntry entry)
        {
            long executionCount = entry.GetExecutionCount();
            long errorCount = entry.GetErrorCount();
            long totalTicks = entry.GetTotalDurationTicks();
            long minTicks = entry.GetMinDurationTicks();
            long maxTicks = entry.GetMaxDurationTicks();

            return new ConsumerMetricsSnapshot.HandlerMetrics(
                entry.HandlerName,
                executionCount,
                errorCount,
                totalTicks,
                executionCount > 0 ? minTicks : 0,
                executionCount > 0 ? maxTicks : 0
            );
        }
    }
}
