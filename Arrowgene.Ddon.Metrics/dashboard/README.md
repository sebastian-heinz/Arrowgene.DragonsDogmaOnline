# DDON Server Metrics Dashboard

A real-time metrics dashboard for Dragon's Dogma Online servers, built with [Observable Framework](https://observablehq.com/framework/) and D3/Plot.

## Overview

The dashboard visualizes server telemetry collected by the `Arrowgene.Ddon.Metrics` module. It displays metrics for **game** and **login** servers including:

- **Connections** — active, peak, accepted, rejected, disconnected, timed out
- **Throughput** — handlers executed per second, errors per second
- **Network** — send/receive rates (KB/s), total bytes transferred
- **Packet Lifecycle** — latency histograms broken down by stage: queue wait, parse + dispatch, handler execution
- **Handler Analysis** — per-handler execution counts, error counts, and duration statistics (avg/min/max)
- **System** — uptime tracking

## Project Structure

```
dashboard/
├── src/
│   ├── index.md              # Main dashboard page (Observable markdown + JS)
│   ├── metrics-config.js     # Server list, color map, and metric file names
│   └── snapshot/             # Runtime metric data (per-server JSON files)
│       ├── game/
│       └── login/
├── scripts/
│   ├── copy-dist.mjs         # Post-build: copies dist to WebServer static files
│   └── generate-dummy-metrics.mjs  # Generates 24h of synthetic metric data
├── dist/                     # Built dashboard output
├── observablehq.config.ts    # Observable Framework configuration
└── package.json
```

## Development

### Prerequisites

- Node.js

### Install dependencies

```bash
npm install
```

### Generate dummy data

Creates 24 hours of realistic synthetic metrics (1-second resolution) for both game and login servers:

```bash
npm run generate:dummy-data
```

Options:

| Flag | Description |
|---|---|
| `--seed <number>` | Override the deterministic random seed |
| `--end <iso-date>` | End timestamp for the 24-hour window (defaults to now) |
| `--output-dir <path>` | Write to a custom directory instead of `src/` and `dist/` |

### Preview locally

```bash
npm run dev
```

This starts the Observable preview server with hot reload.

### Build

```bash
npm run build
```

Builds the static dashboard into `dist/` and automatically copies it to `Arrowgene.Ddon.WebServer/Files/www/metrics/` (via the `postbuild` script).

## Production

When running the DDON server with metrics enabled, the server writes JSON snapshot files to the configured export path (default: `Files/www/metrics/snapshot/`). The dashboard reads these files at runtime.

### Server configuration

Metrics are configured via `MetricSettings` in the server settings:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` | Enable/disable metrics collection |
| `MetricsSink` | `FileMetricsSink` | Storage backend for collected metrics |
| `SamplingIntervalMs` | `1000` | How often metrics are sampled (ms) |
| `FileMetricsSinkRetentionMin` | `1440` | How long to retain metric data (minutes) |
| `FileMetricsExportPath` | `Files/www/metrics/snapshot` | Where snapshot JSON files are written |
| `FileMetricsExportIntervalMs` | `30000` | How often snapshots are exported to disk (ms) |

### Metric data files

Each server (`game`, `login`) produces the following JSON files in its snapshot directory:

| File | Content |
|---|---|
| `timeseries.json` | Time-series samples (connections, throughput, network, uptime) |
| `handlers.json` | Per-handler execution stats |
| `duration_histogram.json` | Handler execution duration buckets |
| `parse_histogram.json` | Parse + dispatch duration buckets |
| `queue_delay_histogram.json` | Queue wait duration buckets |
| `received_handler_duration_histogram.json` | Full received-to-handled duration buckets |

## Adding a new server

1. Add the server name to the `servers` array in `src/metrics-config.js`
2. Assign it a color in `colorMap`
3. Ensure the server writes its metrics to `snapshot/<server-name>/`
