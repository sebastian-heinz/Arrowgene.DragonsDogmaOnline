using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Arrowgene.Logging;
using Arrowgene.Networking.Metrics;
using Arrowgene.Networking.SAEAServer.Metric;

namespace Arrowgene.Ddon.Metrics;

public sealed class FileMetricsSink : IMetricsSink<DdonServerMetricsSnapshot>
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(FileMetricsSink));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly MemoryMetricsSink _memorySink;
    private readonly string _serverDirectory;
    private readonly Timer _exportTimer;
    private int _flushing;

    public FileMetricsSink(TimeSpan retention, string outputDirectory, string serverName, int exportIntervalMs)
    {
        _memorySink = new MemoryMetricsSink(retention);
        _serverDirectory = Path.Combine(outputDirectory, serverName);
        _exportTimer = new Timer(_ => Flush(), null, exportIntervalMs, exportIntervalMs);
    }

    public void Write(DdonServerMetricsSnapshot snapshot)
    {
        _memorySink.Write(snapshot);
    }

    public void Dispose()
    {
        _exportTimer.Dispose();
        Flush();
        _memorySink.Dispose();
    }

    private void Flush()
    {
        if (Interlocked.CompareExchange(ref _flushing, 1, 0) != 0)
        {
            return;
        }

        try
        {
            var samples = _memorySink.GetSamples();
            if (samples.Count == 0)
            {
                return;
            }

            Directory.CreateDirectory(_serverDirectory);
            WriteTimeseries(samples);
            WriteHandlers(samples);
            WriteDurationHistogram(samples);
            WriteParseHistogram(samples);
            WriteQueueDelayHistogram(samples);
            WriteReceivedDataHandlerDurationHistogram(samples);
        }
        catch (Exception ex)
        {
            Logger.Error($"Metrics file export failed: {ex.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _flushing, 0);
        }
    }

    private void WriteTimeseries(List<DdonServerMetricsSnapshot> samples)
    {
        var rows = new List<Dictionary<string, object>>(samples.Count);

        foreach (DdonServerMetricsSnapshot s in samples)
        {
            var row = new Dictionary<string, object>
            {
                ["timestamp"] = s.TimestampUtc.ToString("o"),
                ["sequenceNumber"] = s.SequenceNumber,
                ["uptimeSeconds"] = Math.Round(s.Uptime.TotalSeconds, 1),
                ["handlersExecutedPerSecond"] = Math.Round(s.HandlersExecutedPerSecond, 2),
                ["handlerErrorsPerSecond"] = Math.Round(s.HandlerErrorsPerSecond, 2),
                ["totalHandlersExecuted"] = s.DdonConsumerMetrics.HandlersExecuted,
                ["totalHandlerErrors"] = s.DdonConsumerMetrics.HandlerErrors,
                ["activeConnections"] = s.TcpServerMetrics.ActiveConnections,
                ["peakActiveConnections"] = s.TcpServerMetrics.PeakActiveConnections,
                ["acceptedConnections"] = s.TcpServerMetrics.AcceptedConnections,
                ["rejectedConnections"] = s.TcpServerMetrics.RejectedConnections,
                ["disconnectedConnections"] = s.TcpServerMetrics.DisconnectedConnections,
                ["timedOutConnections"] = s.TcpServerMetrics.TimedOutConnections,
                ["bytesSent"] = s.TcpServerMetrics.BytesSent,
                ["bytesReceived"] = s.TcpServerMetrics.BytesReceived,
                ["sendBytesPerSecond"] = Math.Round(s.TcpServerMetrics.SendBytesPerSecond, 2),
                ["receiveBytesPerSecond"] = Math.Round(s.TcpServerMetrics.ReceiveBytesPerSecond, 2)
            };
            rows.Add(row);
        }

        WriteJson(rows, Path.Combine(_serverDirectory, "timeseries.json"));
    }

    private void WriteHandlers(List<DdonServerMetricsSnapshot> samples)
    {
        DdonServerMetricsSnapshot latest = samples[^1];
        var handlers = latest.DdonConsumerMetrics.Handlers;

        var rows = new List<Dictionary<string, object>>(handlers.Count);
        foreach (var kvp in handlers.OrderByDescending(h => h.Value.ExecutionCount))
        {
            DdonConsumerMetricsSnapshot.HandlerMetrics m = kvp.Value;
            rows.Add(new Dictionary<string, object>
            {
                ["handlerName"] = m.HandlerName,
                ["executionCount"] = m.ExecutionCount,
                ["errorCount"] = m.ErrorCount,
                ["avgDurationMs"] = Math.Round(m.AvgDuration.TotalMilliseconds, 3),
                ["minDurationMs"] = Math.Round(m.MinDuration.TotalMilliseconds, 3),
                ["maxDurationMs"] = Math.Round(m.MaxDuration.TotalMilliseconds, 3)
            });
        }

        WriteJson(rows, Path.Combine(_serverDirectory, "handlers.json"));
    }

    private void WriteDurationHistogram(List<DdonServerMetricsSnapshot> samples)
    {
        DdonServerMetricsSnapshot latest = samples[^1];
        ReadOnlySpan<long> buckets = latest.DdonConsumerMetrics.HandlerDurationBuckets.Span;
        WriteDurationHistogram(
            buckets,
            Path.Combine(_serverDirectory, "duration_histogram.json"));
    }

    private void WriteParseHistogram(List<DdonServerMetricsSnapshot> samples)
    {
        DdonServerMetricsSnapshot latest = samples[^1];
        ReadOnlySpan<long> buckets = latest.DdonConsumerMetrics.ParseDurationBuckets.Span;
        WriteDurationHistogram(
            buckets,
            Path.Combine(_serverDirectory, "parse_histogram.json"));
    }

    private void WriteQueueDelayHistogram(List<DdonServerMetricsSnapshot> samples)
    {
        DdonServerMetricsSnapshot latest = samples[^1];
        var consumerMetrics = latest.TcpServerMetrics.ConsumerMetrics;
        if (consumerMetrics == null)
        {
            return;
        }

        ReadOnlySpan<long> buckets = consumerMetrics.Value.ReceivedDataQueueDelayBuckets.Span;
        WriteConsumerHistogram(buckets, Path.Combine(_serverDirectory, "queue_delay_histogram.json"));
    }

    private void WriteReceivedDataHandlerDurationHistogram(List<DdonServerMetricsSnapshot> samples)
    {
        DdonServerMetricsSnapshot latest = samples[^1];
        var consumerMetrics = latest.TcpServerMetrics.ConsumerMetrics;
        if (consumerMetrics == null)
        {
            return;
        }

        ReadOnlySpan<long> buckets = consumerMetrics.Value.ReceivedDataHandlerDurationBuckets.Span;
        WriteConsumerHistogram(buckets, Path.Combine(_serverDirectory, "received_handler_duration_histogram.json"));
    }

    private void WriteConsumerHistogram(ReadOnlySpan<long> buckets, string path)
    {
        var rows = new List<Dictionary<string, object>>(MetricBucketDefinitions.DurationBucketNames.Count);
        for (int i = 0; i < MetricBucketDefinitions.DurationBucketNames.Count && i < buckets.Length; i++)
        {
            rows.Add(new Dictionary<string, object>
            {
                ["bucket"] = MetricBucketDefinitions.DurationBucketNames[i],
                ["count"] = buckets[i]
            });
        }

        WriteJson(rows, path);
    }

    private void WriteDurationHistogram(ReadOnlySpan<long> buckets, string path)
    {
        WriteConsumerHistogram(buckets, path);
    }

    private static void WriteJson(object data, string path)
    {
        string json = JsonSerializer.Serialize(data, JsonOptions);
        string tempPath = path + ".tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, path, overwrite: true);
    }
}
