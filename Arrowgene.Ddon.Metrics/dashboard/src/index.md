---
title: DDON Server Metrics
toc: false
---

```js
import {colorMap, metricFileNames, servers} from "./metrics-config.js";

const metricFiles = Object.fromEntries(
  servers.map(server => [
    server,
    Object.fromEntries(
      Object.entries(metricFileNames).map(([metric, fileName]) => [
        metric,
        `snapshot/${server}/${fileName}`
      ])
    )
  ])
);
```

```js
function serverToggleInput() {
  const active = new Set([servers[0]]);
  const container = document.createElement("div");
  container.className = "toggle-group";
  container.value = [...active];

  function render() {
    container.innerHTML = "";
    servers.forEach(s => {
      const btn = document.createElement("button");
      const isActive = active.has(s);
      btn.className = "toggle-btn" + (isActive ? " active" : "");
      btn.style.setProperty("--btn-color", colorMap[s] ?? "#888");
      btn.innerHTML = `<span class="toggle-dot" style="background:${isActive ? colorMap[s] : "transparent"};"></span>${s}`;
      btn.onclick = () => {
        if (active.has(s)) { if (active.size > 1) active.delete(s); }
        else active.add(s);
        container.value = [...active];
        render();
        container.dispatchEvent(new Event("input", {bubbles: true}));
      };
      container.appendChild(btn);
    });
  }
  render();
  return container;
}
```

```js
async function readMetric(path) {
  const response = await fetch(path);
  if (response.ok) return response.json();

  const previewResponse = await fetch(`/_file/${path}`);
  if (previewResponse.ok) return previewResponse.json();

  return [];
}

const entries = await Promise.all(selected.map(async (s) => {
  const files = metricFiles[s];
  const [timeseries, handlers, duration_histogram, parse_histogram, queue_delay_histogram, received_handler_duration_histogram] = await Promise.all([
    readMetric(files.timeseries),
    readMetric(files.handlers),
    readMetric(files.durationHistogram),
    readMetric(files.parseHistogram),
    readMetric(files.queueDelayHistogram),
    readMetric(files.receivedHandlerDurationHistogram),
  ]);
  return [s, {timeseries, handlers, duration_histogram, parse_histogram, queue_delay_histogram, received_handler_duration_histogram}];
}));
const metrics = Object.fromEntries(entries);
```

```js
const allTimeseries = selected.flatMap(s => metrics[s].timeseries.map(d => ({...d, server: s})));
const allHandlers = selected.flatMap(s => metrics[s].handlers.map(d => ({...d, server: s})));
const allHistogram = selected.flatMap(s => metrics[s].duration_histogram.map(d => ({...d, server: s})));
const colorDomain = selected;
const colorRange = selected.map(s => colorMap[s] ?? "#888");

// Bucket label ordering (all histograms use the same scheme)
const bucketOrder = ["0..100us", "100us..500us", "500us..1ms", "1ms..5ms", "5ms..10ms", "10ms..50ms", "50ms..100ms", "100ms..250ms", "250ms..500ms", "500ms..1s", "1s..2s", "2s..5s", "5s..10s", "10s..30s", "30s..1m", "1m..2m", "2m..5m", "5m..10m", "10m..30m", "30m..1h+"];

// Packet lifecycle: tag each histogram with its stage and server
const stageColorMap = {"Queue Wait": "#ffab40", "Parse + Dispatch": "#ab47bc", "Handler Execution": "#26c6da"};
const allLifecycle = selected.flatMap(s => [
  ...metrics[s].queue_delay_histogram.map(d => ({...d, stage: "Queue Wait", server: s})),
  ...metrics[s].parse_histogram.map(d => ({...d, stage: "Parse + Dispatch", server: s})),
  ...metrics[s].duration_histogram.map(d => ({...d, stage: "Handler Execution", server: s})),
]);
const allQueueDelay = selected.flatMap(s => metrics[s].queue_delay_histogram.map(d => ({...d, server: s})));
const allParse = selected.flatMap(s => metrics[s].parse_histogram.map(d => ({...d, server: s})));
const allReceivedHandlerDuration = selected.flatMap(s => metrics[s].received_handler_duration_histogram.map(d => ({...d, server: s})));
const chartTextColor = "#9fb4ce";
const timeSeriesPlotStyle = {fontSize: "12px", color: chartTextColor};
const histogramPlotStyle = {fontSize: "12.5px", color: chartTextColor};
const histogramXAxis = {label: null, tickRotate: -55, padding: 0.2, domain: bucketOrder};
const histogramMarginBottom = 92;
```

```js
function serverSummary(s) {
  const ts = metrics[s]?.timeseries ?? [];
  const latest = ts.length > 0 ? ts[ts.length - 1] : null;
  return {
    server: s,
    color: colorMap[s] ?? "#888",
    uptime: latest ? (latest.uptimeSeconds / 3600).toFixed(1) : "—",
    connections: latest ? latest.activeConnections : "—",
    peakConnections: latest ? latest.peakActiveConnections : "—",
    handlersPerSec: latest ? latest.handlersExecutedPerSecond.toFixed(1) : "—",
    errorsPerSec: latest ? latest.handlerErrorsPerSecond.toFixed(2) : "—",
    totalHandlers: latest ? latest.totalHandlersExecuted.toLocaleString() : "—",
    totalErrors: latest ? latest.totalHandlerErrors.toLocaleString() : "—",
    accepted: latest ? latest.acceptedConnections.toLocaleString() : "—",
    rejected: latest ? latest.rejectedConnections.toLocaleString() : "—",
    disconnected: latest ? latest.disconnectedConnections.toLocaleString() : "—",
    timedOut: latest ? latest.timedOutConnections.toLocaleString() : "—",
    sendKBps: latest ? (latest.sendBytesPerSecond / 1024).toFixed(1) : "—",
    recvKBps: latest ? (latest.receiveBytesPerSecond / 1024).toFixed(1) : "—",
    totalSent: latest ? fmtBytes(latest.bytesSent) : "—",
    totalRecv: latest ? fmtBytes(latest.bytesReceived) : "—",
  };
}

function fmtBytes(b) {
  if (b >= 1073741824) return (b / 1073741824).toFixed(2) + " GB";
  if (b >= 1048576) return (b / 1048576).toFixed(1) + " MB";
  if (b >= 1024) return (b / 1024).toFixed(1) + " KB";
  return b + " B";
}

function tsChart(opts) {
  return Plot.plot({
    width: opts.width ?? width,
    height: opts.height ?? 240,
    style: timeSeriesPlotStyle,
    color: {domain: colorDomain, range: colorRange, legend: opts.legend !== false},
    x: {type: "utc", label: null},
    y: {label: opts.yLabel, grid: true, nice: true},
    marks: [
      Plot.ruleY([0], {stroke: "#1e2a3a"}),
      Plot.areaY(allTimeseries, {x: d => new Date(d.timestamp), y: opts.y, fill: "server", fillOpacity: 0.08}),
      Plot.lineY(allTimeseries, {x: d => new Date(d.timestamp), y: opts.y, stroke: "server", strokeWidth: 1.2}),
      Plot.tip(allTimeseries, Plot.pointerX({x: d => new Date(d.timestamp), y: opts.y, stroke: "server"})),
    ]
  });
}

const summaries = selected.map(serverSummary);
```

<div class="dash-title">
  <span class="title-sub">SERVER METRICS</span>
</div>

<div class="server-panel selector-panel" style="border-color: var(--crush-border);">
  <div class="panel-header" style="border-bottom-color: var(--crush-border);">
    <span class="panel-indicator" style="background: var(--crush-muted); box-shadow: 0 0 8px rgba(107,125,148,0.4);"></span>
    <span class="panel-name" style="color: var(--crush-text);">Servers</span>
  </div>
  <div class="selector-content">

```js
const selected = view(serverToggleInput());
```

  </div>
</div>

```js
html`<div class="overview-grid" style="grid-template-columns: repeat(${selected.length}, 1fr);">
${summaries.map(s => html`
<div class="server-panel" style="border-color: ${s.color}30;">
  <div class="panel-header" style="border-bottom-color: ${s.color}30;">
    <span class="panel-indicator" style="background:${s.color}; box-shadow: 0 0 8px ${s.color}60;"></span>
    <span class="panel-name" style="color:${s.color};">${s.server}</span>
    <span class="panel-uptime">${s.uptime}h</span>
  </div>
  <div class="panel-grid">
    <div class="metric">
      <div class="metric-val" style="color:${s.color};">${s.connections}</div>
      <div class="metric-label">CONN</div>
      <div class="metric-sub">peak ${s.peakConnections}</div>
    </div>
    <div class="metric">
      <div class="metric-val" style="color:${s.color};">${s.handlersPerSec}</div>
      <div class="metric-label">HND/S</div>
      <div class="metric-sub">${s.totalHandlers}</div>
    </div>
    <div class="metric">
      <div class="metric-val ${Number(s.errorsPerSec) > 0.5 ? "val-alert" : ""}">${s.errorsPerSec}</div>
      <div class="metric-label">ERR/S</div>
      <div class="metric-sub">${s.totalErrors}</div>
    </div>
    <div class="metric">
      <div class="metric-val metric-val-sm">${s.sendKBps}</div>
      <div class="metric-label">TX KB/S</div>
      <div class="metric-sub">${s.totalSent}</div>
    </div>
    <div class="metric">
      <div class="metric-val metric-val-sm">${s.recvKBps}</div>
      <div class="metric-label">RX KB/S</div>
      <div class="metric-sub">${s.totalRecv}</div>
    </div>
    <div class="metric">
      <div class="metric-val metric-val-sm">${s.accepted}</div>
      <div class="metric-label">ACCEPT</div>
      <div class="metric-sub">rej ${s.rejected} / to ${s.timedOut}</div>
    </div>
  </div>
</div>
`)}</div>`
```

<div class="section-bar"><span>THROUGHPUT</span></div>

<div class="grid grid-cols-2">
<div class="card chart-card">
<div class="chart-title">HANDLERS / SEC</div>

```js
tsChart({y: "handlersExecutedPerSecond", yLabel: "hnd/s"})
```

</div>
<div class="card chart-card">
<div class="chart-title">ERRORS / SEC</div>

```js
tsChart({y: "handlerErrorsPerSecond", yLabel: "err/s"})
```

</div>
</div>

<div class="grid grid-cols-2">
<div class="card chart-card">
<div class="chart-title">TOTAL HANDLERS</div>

```js
tsChart({y: "totalHandlersExecuted", yLabel: "cumulative", legend: false})
```

</div>
<div class="card chart-card">
<div class="chart-title">TOTAL ERRORS</div>

```js
tsChart({y: "totalHandlerErrors", yLabel: "cumulative", legend: false})
```

</div>
</div>

<div class="section-bar"><span>CONNECTIONS</span></div>

<div class="grid grid-cols-2">
<div class="card chart-card">
<div class="chart-title">ACTIVE</div>

```js
tsChart({y: "activeConnections", yLabel: "connections"})
```

</div>
<div class="card chart-card">
<div class="chart-title">PEAK</div>

```js
tsChart({y: "peakActiveConnections", yLabel: "peak", legend: false})
```

</div>
</div>

<div class="grid grid-cols-4">
<div class="card chart-card">
<div class="chart-title">ACCEPTED</div>

```js
tsChart({y: "acceptedConnections", yLabel: null, height: 160, legend: false})
```

</div>
<div class="card chart-card">
<div class="chart-title">DISCONNECTED</div>

```js
tsChart({y: "disconnectedConnections", yLabel: null, height: 160, legend: false})
```

</div>
<div class="card chart-card">
<div class="chart-title">REJECTED</div>

```js
tsChart({y: "rejectedConnections", yLabel: null, height: 160, legend: false})
```

</div>
<div class="card chart-card">
<div class="chart-title">TIMED OUT</div>

```js
tsChart({y: "timedOutConnections", yLabel: null, height: 160, legend: false})
```

</div>
</div>

<div class="section-bar"><span>NETWORK</span></div>

<div class="grid grid-cols-2">
<div class="card chart-card">
<div class="chart-title">TX RATE</div>

```js
tsChart({y: d => d.sendBytesPerSecond / 1024, yLabel: "KB/s"})
```

</div>
<div class="card chart-card">
<div class="chart-title">RX RATE</div>

```js
tsChart({y: d => d.receiveBytesPerSecond / 1024, yLabel: "KB/s", legend: false})
```

</div>
</div>

<div class="grid grid-cols-2">
<div class="card chart-card">
<div class="chart-title">TOTAL SENT</div>

```js
tsChart({y: d => d.bytesSent / 1048576, yLabel: "MB", legend: false})
```

</div>
<div class="card chart-card">
<div class="chart-title">TOTAL RECEIVED</div>

```js
tsChart({y: d => d.bytesReceived / 1048576, yLabel: "MB", legend: false})
```

</div>
</div>

<div class="section-bar"><span>PACKET LIFECYCLE</span></div>

<div class="card chart-card">
<div class="chart-title">PIPELINE OVERVIEW &mdash; TCP RECEIVE &rarr; QUEUE &rarr; PARSE &rarr; HANDLE</div>
<div class="pipeline-diagram">
  <div class="pipeline-stage" style="border-color: #ffab4050;">
    <div class="pipeline-label" style="color: #ffab40;">QUEUE WAIT</div>
    <div class="pipeline-desc">enqueue &rarr; dequeue</div>
  </div>
  <div class="pipeline-arrow">&rarr;</div>
  <div class="pipeline-stage" style="border-color: #ab47bc50;">
    <div class="pipeline-label" style="color: #ab47bc;">PARSE + DISPATCH</div>
    <div class="pipeline-desc">decrypt, parse, lookup</div>
  </div>
  <div class="pipeline-arrow">&rarr;</div>
  <div class="pipeline-stage" style="border-color: #26c6da50;">
    <div class="pipeline-label" style="color: #26c6da;">HANDLER EXECUTION</div>
    <div class="pipeline-desc">business logic</div>
  </div>
</div>

```js
Plot.plot({
  width,
  height: 340,
  marginBottom: histogramMarginBottom,
  style: histogramPlotStyle,
  color: {
    domain: ["Queue Wait", "Parse + Dispatch", "Handler Execution"],
    range: ["#ffab40", "#ab47bc", "#26c6da"],
    legend: true
  },
  x: {...histogramXAxis, padding: 0.15},
  y: {label: "count", grid: true},
  fx: selected.length > 1 ? {label: null} : undefined,
  marks: [
    Plot.barY(
      allLifecycle,
      {
        x: "bucket",
        y: "count",
        fill: "stage",
        fx: selected.length > 1 ? "server" : undefined,
        tip: true,
        sort: {x: null}
      }
    ),
    Plot.ruleY([0], {stroke: "#1e2a3a"}),
  ]
})
```

</div>

<div class="grid grid-cols-3">
<div class="card chart-card">
<div class="chart-title" style="color: #ffab40;">QUEUE WAIT</div>

```js
Plot.plot({
  width,
  height: 250,
  marginBottom: histogramMarginBottom,
  style: histogramPlotStyle,
  color: {domain: colorDomain, range: colorRange, legend: selected.length > 1},
  x: histogramXAxis,
  y: {label: "count", grid: true},
  marks: [
    Plot.barY(allQueueDelay, {x: "bucket", y: "count", fill: "server", tip: true, sort: {x: null}}),
    Plot.ruleY([0], {stroke: "#1e2a3a"}),
  ]
})
```

</div>
<div class="card chart-card">
<div class="chart-title" style="color: #ab47bc;">PARSE + DISPATCH</div>

```js
Plot.plot({
  width,
  height: 250,
  marginBottom: histogramMarginBottom,
  style: histogramPlotStyle,
  color: {domain: colorDomain, range: colorRange, legend: false},
  x: histogramXAxis,
  y: {label: "count", grid: true},
  marks: [
    Plot.barY(allParse, {x: "bucket", y: "count", fill: "server", tip: true, sort: {x: null}}),
    Plot.ruleY([0], {stroke: "#1e2a3a"}),
  ]
})
```

</div>
<div class="card chart-card">
<div class="chart-title" style="color: #26c6da;">HANDLER EXECUTION</div>

```js
Plot.plot({
  width,
  height: 250,
  marginBottom: histogramMarginBottom,
  style: histogramPlotStyle,
  color: {domain: colorDomain, range: colorRange, legend: false},
  x: histogramXAxis,
  y: {label: "count", grid: true},
  marks: [
    Plot.barY(allHistogram, {x: "bucket", y: "count", fill: "server", tip: true, sort: {x: null}}),
    Plot.ruleY([0], {stroke: "#1e2a3a"}),
  ]
})
```

</div>
</div>

<div class="card chart-card">
<div class="chart-title">RECEIVED DATA HANDLER DURATION (NETWORKING LAYER)</div>

```js
Plot.plot({
  width,
  height: 250,
  marginBottom: histogramMarginBottom,
  style: histogramPlotStyle,
  color: {domain: colorDomain, range: colorRange, legend: true},
  x: histogramXAxis,
  y: {label: "count", grid: true},
  marks: [
    Plot.barY(allReceivedHandlerDuration, {x: "bucket", y: "count", fill: "server", tip: true, sort: {x: null}}),
    Plot.ruleY([0], {stroke: "#1e2a3a"}),
  ]
})
```

</div>

<div class="section-bar"><span>HANDLER ANALYSIS</span></div>

<div class="card chart-card">
<div class="chart-title">HANDLER PERFORMANCE</div>

```js
Inputs.table(allHandlers, {
  columns: ["server", "handlerName", "executionCount", "errorCount", "avgDurationMs", "minDurationMs", "maxDurationMs"],
  header: {
    server: "Server",
    handlerName: "Handler",
    executionCount: "Exec",
    errorCount: "Err",
    avgDurationMs: "Avg ms",
    minDurationMs: "Min ms",
    maxDurationMs: "Max ms"
  },
  sort: "executionCount",
  reverse: true,
  format: {
    executionCount: d => d.toLocaleString(),
    errorCount: d => d.toLocaleString(),
    avgDurationMs: d => d.toFixed(3),
    minDurationMs: d => d.toFixed(3),
    maxDurationMs: d => d.toFixed(1),
  },
  width: {
    handlerName: 280
  }
})
```

</div>

<div class="section-bar"><span>SYSTEM</span></div>

<div class="card chart-card">
<div class="chart-title">UPTIME</div>

```js
tsChart({y: d => d.uptimeSeconds / 3600, yLabel: "hours", height: 160})
```

</div>

<style>
/* ===== CRUSH THEME ===== */

:root {
  --crush-bg: #0b0e14;
  --crush-surface: #0f1318;
  --crush-border: #1a2030;
  --crush-text: #e2e8f0;
  --crush-muted: #6b7d94;
  --crush-cyan: #00e5ff;
  --crush-orange: #ff6e40;
  --crush-red: #ff3d5a;
  --crush-green: #00e676;
  --crush-glow: 0 0 20px rgba(0, 229, 255, 0.08);
}

#observablehq-main {
  --theme-background: var(--crush-bg) !important;
  --theme-foreground: var(--crush-text) !important;
  --theme-foreground-muted: var(--crush-muted) !important;
  --theme-foreground-faint: #1a2030 !important;
  --theme-foreground-faintest: #131820 !important;
  font-family: "JetBrains Mono", "Fira Code", "SF Mono", "Cascadia Code", ui-monospace, monospace !important;
}

h1, h2, h3 {
  font-family: inherit !important;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--crush-text) !important;
}

h2 {
  font-size: 0.75rem !important;
  font-weight: 600 !important;
  color: var(--crush-muted) !important;
  margin-top: 0 !important;
  margin-bottom: 0.75rem !important;
}

/* Dashboard title */
.dash-title {
  text-align: center;
  margin: -0.5rem 0 1.25rem;
}
.title-text {
  font-size: 1.5rem;
  font-weight: 800;
  letter-spacing: 0.2em;
  color: var(--crush-cyan);
  text-shadow: 0 0 30px rgba(0, 229, 255, 0.3), 0 0 60px rgba(0, 229, 255, 0.1);
}
.title-sub {
  font-size: 0.65rem;
  font-weight: 400;
  letter-spacing: 0.3em;
  color: var(--crush-muted);
  margin-left: 0.75rem;
}

/* Let Observable form wrapper be transparent */
form:has(.toggle-group) {
  display: contents !important;
}
form label:has(+ .toggle-group) {
  display: none !important;
}

/* Selector panel */
.selector-panel {
  margin-bottom: 1rem;
}
.selector-content {
  padding: 0.6rem 0.75rem;
}

/* Toggle buttons */
.toggle-group {
  display: flex;
  gap: 0.5rem;
}
.toggle-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.4rem 0.75rem;
  border: 1px solid var(--crush-border);
  border-radius: 4px;
  background: transparent;
  color: var(--crush-muted);
  font-family: "JetBrains Mono", "Fira Code", "SF Mono", ui-monospace, monospace;
  font-size: 0.7rem;
  font-weight: 600;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  cursor: pointer;
  transition: all 0.15s ease;
}
.toggle-btn:hover {
  border-color: var(--btn-color);
  color: var(--crush-text);
}
.toggle-btn.active {
  border-color: var(--btn-color);
  background: color-mix(in srgb, var(--btn-color) 12%, transparent);
  color: var(--btn-color);
  box-shadow: 0 0 8px color-mix(in srgb, var(--btn-color) 30%, transparent);
}
.toggle-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  border: 1.5px solid var(--btn-color);
  flex-shrink: 0;
  transition: all 0.15s ease;
}
.toggle-btn.active .toggle-dot {
  box-shadow: 0 0 6px var(--btn-color);
}

/* Overview panels - horizontal */
.overview-grid {
  display: grid;
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}

.server-panel {
  background: var(--crush-surface);
  border: 1px solid;
  border-radius: 4px;
  overflow: hidden;
}

.panel-header {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.6rem 0.75rem;
  border-bottom: 1px solid;
  background: rgba(0, 0, 0, 0.3);
}
.panel-indicator {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  flex-shrink: 0;
}
.panel-name {
  font-size: 0.8rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
}
.panel-uptime {
  margin-left: auto;
  font-size: 0.65rem;
  color: var(--crush-muted);
  letter-spacing: 0.05em;
}

.panel-grid {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 0;
}

.metric {
  padding: 0.6rem 0.65rem;
  border-right: 1px solid var(--crush-border);
  text-align: center;
}
.metric:last-child {
  border-right: none;
}
.metric-val {
  font-size: 1.4rem;
  font-weight: 700;
  line-height: 1;
  letter-spacing: -0.02em;
  color: var(--crush-text);
}
.metric-val-sm {
  font-size: 1.05rem;
}
.metric-label {
  font-size: 0.55rem;
  font-weight: 600;
  letter-spacing: 0.12em;
  color: var(--crush-muted);
  margin-top: 0.2rem;
}
.metric-sub {
  font-size: 0.55rem;
  color: #4a5e75;
  margin-top: 0.15rem;
  letter-spacing: 0.02em;
}
.val-alert {
  color: var(--crush-red) !important;
  text-shadow: 0 0 12px rgba(255, 61, 90, 0.4);
}

/* Section bars */
.section-bar {
  border-top: 1px solid var(--crush-border);
  margin: 1.75rem 0 1rem;
  padding-top: 0.75rem;
}
.section-bar span {
  font-size: 0.6rem;
  font-weight: 700;
  letter-spacing: 0.2em;
  color: var(--crush-muted);
}

/* Chart cards */
.card.chart-card {
  background: var(--crush-surface) !important;
  border: 1px solid var(--crush-border) !important;
  border-radius: 4px !important;
  padding: 0.75rem !important;
}
.chart-title {
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  color: var(--crush-muted);
  margin-bottom: 0.65rem;
  text-transform: uppercase;
  line-height: 1.35;
}

/* Plot overrides */
[class*="plot-"] text,
figure text {
  fill: #8a9db5 !important;
  font-size: 12px !important;
  font-weight: 500 !important;
}
figure [aria-label="rule"] line {
  stroke: var(--crush-border) !important;
}

/* Table */
table {
  font-size: 0.7rem !important;
}
table th {
  font-size: 0.6rem !important;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: var(--crush-muted) !important;
  border-bottom-color: var(--crush-border) !important;
}
table td {
  border-bottom-color: var(--crush-border) !important;
  color: var(--crush-text) !important;
}
table tr:hover td {
  background: rgba(0, 229, 255, 0.03) !important;
}

/* Scrollbar */
::-webkit-scrollbar {
  width: 4px;
  height: 4px;
}
::-webkit-scrollbar-track {
  background: var(--crush-bg);
}
::-webkit-scrollbar-thumb {
  background: var(--crush-border);
  border-radius: 2px;
}

/* Pipeline diagram */
.pipeline-diagram {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  margin: 0.75rem 0 1rem;
}
.pipeline-stage {
  border: 1px solid;
  border-radius: 4px;
  padding: 0.6rem 1.1rem;
  text-align: center;
  background: rgba(0, 0, 0, 0.2);
  min-width: 160px;
}
.pipeline-label {
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  line-height: 1.25;
}
.pipeline-desc {
  font-size: 0.58rem;
  color: var(--crush-muted);
  margin-top: 0.2rem;
  letter-spacing: 0.03em;
  line-height: 1.35;
}
.pipeline-arrow {
  color: var(--crush-muted);
  font-size: 1.15rem;
  opacity: 0.4;
}

/* Grid gap tightening */
.grid {
  gap: 0.75rem !important;
}
</style>
