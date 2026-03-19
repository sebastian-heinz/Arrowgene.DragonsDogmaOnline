#!/usr/bin/env node

import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const SECONDS_PER_DAY = 24 * 60 * 60;
const DEFAULT_SEED = 20260319;
const DURATION_BUCKETS = [
  "0..100us",
  "100us..500us",
  "500us..1ms",
  "1ms..5ms",
  "5ms..10ms",
  "10ms..50ms",
  "50ms..100ms",
  "100ms..250ms",
  "250ms..500ms",
  "500ms..1s",
  "1s..2s",
  "2s..5s",
  "5s..10s",
  "10s..30s",
  "30s..1m",
  "1m..2m",
  "2m..5m",
  "5m..10m",
  "10m..30m",
  "30m..1h+"
];

const GAME_DURATION_WEIGHTS = [
  0.024, 0.096, 0.19, 0.34, 0.12,
  0.11, 0.042, 0.03, 0.018, 0.011,
  0.006, 0.004, 0.0027, 0.0017, 0.0011,
  0.00072, 0.00046, 0.00024, 0.0001, 0.00003
];

const GAME_PARSE_WEIGHTS = [
  0.066, 0.24, 0.21, 0.29, 0.09,
  0.055, 0.023, 0.013, 0.007, 0.003,
  0.0015, 0.0008, 0.00045, 0.00021, 0.0001,
  0.00005, 0.00003, 0.000015, 0.000007, 0.000003
];

const GAME_QUEUE_DELAY_WEIGHTS = [
  0.018, 0.075, 0.16, 0.22, 0.14,
  0.16, 0.08, 0.055, 0.04, 0.022,
  0.013, 0.007, 0.004, 0.0024, 0.0014,
  0.0008, 0.00045, 0.00023, 0.00008, 0.00003
];

const GAME_RECEIVED_HANDLER_WEIGHTS = [
  0.028, 0.11, 0.18, 0.29, 0.11,
  0.12, 0.052, 0.038, 0.026, 0.017,
  0.011, 0.006, 0.0037, 0.0022, 0.0013,
  0.00078, 0.00045, 0.00022, 0.00009, 0.00003
];

const LOGIN_DURATION_WEIGHTS = [
  0.045, 0.14, 0.23, 0.28, 0.11,
  0.085, 0.038, 0.023, 0.016, 0.012,
  0.008, 0.005, 0.003, 0.0019, 0.0012,
  0.00072, 0.00042, 0.0002, 0.00008, 0.00002
];

const LOGIN_PARSE_WEIGHTS = [
  0.1, 0.24, 0.22, 0.24, 0.082,
  0.05, 0.025, 0.015, 0.009, 0.005,
  0.0025, 0.0014, 0.00075, 0.00035, 0.00018,
  0.00008, 0.00004, 0.00002, 0.000008, 0.000003
];

const LOGIN_QUEUE_DELAY_WEIGHTS = [
  0.03, 0.11, 0.2, 0.24, 0.11,
  0.09, 0.055, 0.036, 0.026, 0.018,
  0.01, 0.0055, 0.0028, 0.0015, 0.0008,
  0.00042, 0.00022, 0.0001, 0.00004, 0.00001
];

const LOGIN_RECEIVED_HANDLER_WEIGHTS = [
  0.05, 0.15, 0.22, 0.25, 0.1,
  0.08, 0.045, 0.03, 0.022, 0.016,
  0.01, 0.006, 0.0034, 0.0018, 0.00095,
  0.00052, 0.00026, 0.00012, 0.00005, 0.00001
];

const SERVER_PROFILES = {
  game: {
    seedOffset: 17,
    uptimeBaseSeconds: (8 * 24 * 60 * 60) + (3 * 60 * 60) + (17 * 60),
    initialActiveConnections: 286,
    initialPeakConnections: 742,
    minConnections: 120,
    maxConnections: 1200,
    connectionBase: 180,
    connectionSpread: 620,
    connectionJitter: 16,
    connectionCatchUp: 0.24,
    churnBase: 0.9,
    churnLoadFactor: 4.2,
    baseHandlersPerSecond: 70,
    handlersPerConnection: 0.95,
    overloadHandlerBoost: 320,
    receiveBytesPerHandler: 360,
    sendBytesPerHandler: 520,
    receiveOverheadPerConnection: 42,
    sendOverheadPerConnection: 58,
    errorFloor: 0.00006,
    errorIncidentScale: 0.0018,
    rollingWindowSeconds: 20,
    dayPhase: 0.85,
    secondaryPhase: 0.35,
    eveningPeakCenter: (20 * 60 * 60) + (15 * 60),
    eveningPeakWidth: 2.3 * 60 * 60,
    eveningPeakBoost: 0.27,
    maintenanceCenter: (5 * 60 * 60) + (10 * 60),
    maintenanceWidth: 1.4 * 60 * 60,
    maintenanceDrop: 0.11,
    initialCounters: {
      totalHandlers: 42_800_000,
      totalErrors: 5_230,
      acceptedConnections: 98_500,
      rejectedConnections: 148,
      disconnectedConnections: 98_230,
      timedOutConnections: 812,
      bytesSent: 158_400_000_000,
      bytesReceived: 104_800_000_000
    },
    incidents: [
      {
        center: (2 * 60 * 60) + (18 * 60),
        width: 10 * 60,
        activity: -0.08,
        connectionOffset: -35,
        latency: 0.35,
        errors: 0.22,
        rejections: 0.12,
        timeouts: 0.09
      },
      {
        center: (19 * 60 * 60) + (48 * 60),
        width: 24 * 60,
        activity: 0.22,
        connectionOffset: 105,
        latency: 0.16,
        errors: 0.07,
        rejections: 0.03,
        timeouts: 0.015
      },
      {
        center: (21 * 60 * 60) + (9 * 60),
        width: 12 * 60,
        activity: 0.04,
        connectionOffset: 15,
        latency: 0.46,
        errors: 0.32,
        rejections: 0.08,
        timeouts: 0.04
      }
    ],
    histogramWeights: {
      duration: GAME_DURATION_WEIGHTS,
      parse: GAME_PARSE_WEIGHTS,
      queueDelay: GAME_QUEUE_DELAY_WEIGHTS,
      receivedHandlerDuration: GAME_RECEIVED_HANDLER_WEIGHTS
    },
    histogramMultipliers: {
      duration: 1.0,
      parse: 1.03,
      queueDelay: 1.08,
      receivedHandlerDuration: 1.06
    },
    handlers: [
      { name: "C2S_CHARACTER_CHARACTER_MOVE_REQ", share: 0.235, avgDurationMs: 0.058, minFactor: 0.1, maxFactor: 40, errorWeight: 0.1 },
      { name: "C2S_CONNECTION_PING_REQ", share: 0.155, avgDurationMs: 0.016, minFactor: 0.12, maxFactor: 28, errorWeight: 0.03 },
      { name: "C2S_CONTEXT_SET_CONTEXT_REQ", share: 0.095, avgDurationMs: 0.044, minFactor: 0.11, maxFactor: 34, errorWeight: 0.05 },
      { name: "C2S_INSTANCE_GET_CONTEXT_REQ", share: 0.08, avgDurationMs: 0.11, minFactor: 0.12, maxFactor: 42, errorWeight: 0.06 },
      { name: "C2S_STAGE_AREA_JUMP_REQ", share: 0.07, avgDurationMs: 0.19, minFactor: 0.13, maxFactor: 56, errorWeight: 0.08 },
      { name: "C2S_LOBBY_LOBBY_CHAT_MSG_REQ", share: 0.055, avgDurationMs: 0.62, minFactor: 0.1, maxFactor: 70, errorWeight: 0.09 },
      { name: "C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ", share: 0.05, avgDurationMs: 0.95, minFactor: 0.12, maxFactor: 78, errorWeight: 0.1 },
      { name: "C2S_ITEM_USE_BAG_ITEM_REQ", share: 0.045, avgDurationMs: 1.45, minFactor: 0.11, maxFactor: 88, errorWeight: 0.2 },
      { name: "C2S_INSTANCE_ENEMY_KILL_REQ", share: 0.036, avgDurationMs: 3.3, minFactor: 0.16, maxFactor: 64, errorWeight: 0.5 },
      { name: "C2S_PARTY_PARTY_JOIN_REQ", share: 0.026, avgDurationMs: 4.8, minFactor: 0.18, maxFactor: 58, errorWeight: 0.42 },
      { name: "C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ", share: 0.021, avgDurationMs: 6.4, minFactor: 0.2, maxFactor: 48, errorWeight: 0.18 },
      { name: "C2S_CRAFT_START_CRAFT_REQ", share: 0.017, avgDurationMs: 5.6, minFactor: 0.18, maxFactor: 52, errorWeight: 0.16 },
      { name: "C2S_EQUIP_CHANGE_PAWN_EQUIP_REQ", share: 0.013, avgDurationMs: 2.2, minFactor: 0.15, maxFactor: 44, errorWeight: 0.12 },
      { name: "C2S_SYSTEM_GET_SET_ANNOUNCEMENT_REQ", share: 0.012, avgDurationMs: 0.38, minFactor: 0.1, maxFactor: 46, errorWeight: 0.04 },
      { name: "C2S_STAGE_GET_MAP_FOG_INFO_REQ", share: 0.011, avgDurationMs: 2.8, minFactor: 0.17, maxFactor: 42, errorWeight: 0.11 },
      { name: "C2S_PAWN_GET_PAWN_LIST_REQ", share: 0.009, avgDurationMs: 1.2, minFactor: 0.12, maxFactor: 36, errorWeight: 0.08 },
      { name: "C2S_EMOTE_PLAY_REQ", share: 0.008, avgDurationMs: 0.52, minFactor: 0.12, maxFactor: 34, errorWeight: 0.05 },
      { name: "C2S_SYSTEM_RESUME_GAME_REQ", share: 0.007, avgDurationMs: 3.7, minFactor: 0.18, maxFactor: 47, errorWeight: 0.11 }
    ]
  },
  login: {
    seedOffset: 43,
    uptimeBaseSeconds: (21 * 24 * 60 * 60) + (6 * 60 * 60) + (42 * 60),
    initialActiveConnections: 82,
    initialPeakConnections: 214,
    minConnections: 18,
    maxConnections: 320,
    connectionBase: 36,
    connectionSpread: 170,
    connectionJitter: 6,
    connectionCatchUp: 0.28,
    churnBase: 0.45,
    churnLoadFactor: 2.6,
    baseHandlersPerSecond: 18,
    handlersPerConnection: 0.52,
    overloadHandlerBoost: 92,
    receiveBytesPerHandler: 230,
    sendBytesPerHandler: 310,
    receiveOverheadPerConnection: 21,
    sendOverheadPerConnection: 27,
    errorFloor: 0.00012,
    errorIncidentScale: 0.0024,
    rollingWindowSeconds: 16,
    dayPhase: 0.55,
    secondaryPhase: 1.2,
    eveningPeakCenter: (18 * 60 * 60) + (35 * 60),
    eveningPeakWidth: 1.7 * 60 * 60,
    eveningPeakBoost: 0.21,
    maintenanceCenter: (4 * 60 * 60) + (25 * 60),
    maintenanceWidth: 1.1 * 60 * 60,
    maintenanceDrop: 0.08,
    initialCounters: {
      totalHandlers: 7_950_000,
      totalErrors: 2_410,
      acceptedConnections: 38_200,
      rejectedConnections: 119,
      disconnectedConnections: 38_020,
      timedOutConnections: 364,
      bytesSent: 18_600_000_000,
      bytesReceived: 14_900_000_000
    },
    incidents: [
      {
        center: (7 * 60 * 60) + (52 * 60),
        width: 7 * 60,
        activity: 0.18,
        connectionOffset: 48,
        latency: 0.14,
        errors: 0.08,
        rejections: 0.04,
        timeouts: 0.01
      },
      {
        center: (18 * 60 * 60) + (32 * 60),
        width: 10 * 60,
        activity: 0.12,
        connectionOffset: 22,
        latency: 0.34,
        errors: 0.25,
        rejections: 0.16,
        timeouts: 0.06
      }
    ],
    histogramWeights: {
      duration: LOGIN_DURATION_WEIGHTS,
      parse: LOGIN_PARSE_WEIGHTS,
      queueDelay: LOGIN_QUEUE_DELAY_WEIGHTS,
      receivedHandlerDuration: LOGIN_RECEIVED_HANDLER_WEIGHTS
    },
    histogramMultipliers: {
      duration: 1.0,
      parse: 1.02,
      queueDelay: 1.04,
      receivedHandlerDuration: 1.03
    },
    handlers: [
      { name: "C2L_LOGIN_AUTH_REQ", share: 0.2, avgDurationMs: 1.2, minFactor: 0.15, maxFactor: 62, errorWeight: 0.38 },
      { name: "C2L_LOGIN_PING_REQ", share: 0.19, avgDurationMs: 0.02, minFactor: 0.12, maxFactor: 26, errorWeight: 0.02 },
      { name: "C2L_CHARACTER_SELECT_REQ", share: 0.13, avgDurationMs: 0.85, minFactor: 0.16, maxFactor: 54, errorWeight: 0.24 },
      { name: "C2L_CHARACTER_LIST_REQ", share: 0.12, avgDurationMs: 0.52, minFactor: 0.15, maxFactor: 43, errorWeight: 0.12 },
      { name: "C2L_REQUEST_SERVER_STATUS_REQ", share: 0.11, avgDurationMs: 0.12, minFactor: 0.14, maxFactor: 35, errorWeight: 0.04 },
      { name: "C2L_GET_ANNOUNCEMENT_REQ", share: 0.08, avgDurationMs: 0.31, minFactor: 0.14, maxFactor: 34, errorWeight: 0.08 },
      { name: "C2L_REQUEST_EXTENSION_TOKEN_REQ", share: 0.065, avgDurationMs: 1.65, minFactor: 0.18, maxFactor: 64, errorWeight: 0.32 },
      { name: "C2L_HANDSHAKE_REQ", share: 0.055, avgDurationMs: 0.08, minFactor: 0.12, maxFactor: 28, errorWeight: 0.03 },
      { name: "C2L_CREATE_CHARACTER_REQ", share: 0.045, avgDurationMs: 4.1, minFactor: 0.22, maxFactor: 56, errorWeight: 0.11 },
      { name: "C2L_DELETE_CHARACTER_REQ", share: 0.015, avgDurationMs: 3.6, minFactor: 0.2, maxFactor: 51, errorWeight: 0.08 },
      { name: "C2L_REQUEST_WORLD_LIST_REQ", share: 0.05, avgDurationMs: 0.26, minFactor: 0.13, maxFactor: 31, errorWeight: 0.05 },
      { name: "C2L_SESSION_RESUME_REQ", share: 0.04, avgDurationMs: 0.93, minFactor: 0.16, maxFactor: 42, errorWeight: 0.1 }
    ]
  }
};

async function main() {
  const options = parseArgs(process.argv.slice(2));
  if (options.help) {
    printHelp();
    return;
  }

  const seed = parseIntegerOption(options.seed, DEFAULT_SEED, "seed");
  const endTime = options.end ? new Date(options.end) : new Date();
  if (Number.isNaN(endTime.getTime())) {
    throw new Error(`Invalid --end value: ${options.end}`);
  }

  const normalizedEndTime = floorToSecond(endTime);
  const startTime = new Date(normalizedEndTime.getTime() - ((SECONDS_PER_DAY - 1) * 1000));
  const outputRoots = getOutputRoots(options["output-dir"]);
  const summaries = [];

  for (const [serverName, profile] of Object.entries(SERVER_PROFILES)) {
    const dataset = generateServerDataset(serverName, profile, startTime, seed + profile.seedOffset);
    await writeDataset(outputRoots, serverName, dataset.payloads);
    summaries.push(dataset.summary);
  }

  console.log(`Generated ${SECONDS_PER_DAY.toLocaleString()} 1-second samples per server.`);
  console.log(`Time range: ${startTime.toISOString()} -> ${normalizedEndTime.toISOString()}`);
  console.log(`Outputs: ${outputRoots.join(", ")}`);

  for (const summary of summaries) {
    console.log(
      `${summary.server}: ${summary.points.toLocaleString()} samples, ` +
      `${summary.totalHandlers.toLocaleString()} handlers, ` +
      `${summary.totalErrors.toLocaleString()} errors, ` +
      `${formatBytes(summary.bytesSent)} sent, ` +
      `${formatBytes(summary.bytesReceived)} received`
    );
  }
}

function generateServerDataset(serverName, profile, startTime, seed) {
  const random = mulberry32(seed);
  const handlerRateWindow = new RollingAverage(profile.rollingWindowSeconds);
  const errorRateWindow = new RollingAverage(Math.max(8, Math.round(profile.rollingWindowSeconds / 2)));
  const sendRateWindow = new RollingAverage(profile.rollingWindowSeconds);
  const receiveRateWindow = new RollingAverage(profile.rollingWindowSeconds);
  const timeseries = new Array(SECONDS_PER_DAY);

  let activeConnections = profile.initialActiveConnections;
  let peakActiveConnections = Math.max(profile.initialPeakConnections, activeConnections);
  let totalHandlers = profile.initialCounters.totalHandlers;
  let totalErrors = profile.initialCounters.totalErrors;
  let acceptedConnections = profile.initialCounters.acceptedConnections;
  let rejectedConnections = profile.initialCounters.rejectedConnections;
  let disconnectedConnections = profile.initialCounters.disconnectedConnections;
  let timedOutConnections = profile.initialCounters.timedOutConnections;
  let bytesSent = profile.initialCounters.bytesSent;
  let bytesReceived = profile.initialCounters.bytesReceived;

  for (let second = 0; second < SECONDS_PER_DAY; second++) {
    const load = computeLoad(second, profile);
    const incident = computeIncidentEffects(second, profile.incidents);
    const desiredConnections = clamp(
      Math.round(
        profile.connectionBase
        + (profile.connectionSpread * load)
        + incident.connectionOffset
        + randomCentered(random, profile.connectionJitter)
      ),
      profile.minConnections,
      profile.maxConnections
    );

    const churn = profile.churnBase + (load * profile.churnLoadFactor) + (incident.latency * 3.4);
    const delta = desiredConnections - activeConnections;

    let accepts = 0;
    let disconnects = 0;

    if (delta >= 0) {
      accepts = Math.max(0, Math.round((delta * profile.connectionCatchUp) + (churn * (0.7 + random()))));
      disconnects = Math.max(0, Math.round(churn * 0.12 * random()));
    } else {
      disconnects = Math.max(0, Math.round((-delta * profile.connectionCatchUp) + (churn * (0.8 + random()))));
      accepts = Math.max(0, Math.round(churn * 0.08 * random()));
    }

    if (random() < (0.009 + (load * 0.012))) {
      accepts += randomInt(random, 1, 3);
    }

    if (random() < (0.007 + (incident.latency * 0.02))) {
      disconnects += randomInt(random, 1, 2);
    }

    const timeoutRisk = incident.timeouts + Math.max(0, load - 0.92) * 0.07;
    const timedOutThisSecond = timeoutRisk > 0
      ? Math.max(0, Math.round(timeoutRisk * (0.3 + random()) * 2.8))
      : 0;

    const rejectionRisk = incident.rejections + Math.max(0, load - 0.95) * 0.09;
    const rejectedThisSecond = rejectionRisk > 0
      ? Math.max(0, Math.round(rejectionRisk * (0.5 + random()) * 2.4))
      : 0;

    activeConnections = clamp(
      activeConnections + accepts - disconnects - timedOutThisSecond,
      profile.minConnections,
      profile.maxConnections
    );

    peakActiveConnections = Math.max(peakActiveConnections, activeConnections);
    acceptedConnections += accepts;
    rejectedConnections += rejectedThisSecond;
    disconnectedConnections += disconnects;
    timedOutConnections += timedOutThisSecond;

    const handlerTarget =
      profile.baseHandlersPerSecond
      + (activeConnections * profile.handlersPerConnection * (0.55 + (load * 0.8)))
      + (incident.activity * profile.overloadHandlerBoost)
      + (incident.latency * 38);

    let handlersThisSecond = Math.max(1, Math.round(handlerTarget * (0.92 + (random() * 0.18))));
    if (incident.activity > 0.18 && random() < 0.06) {
      handlersThisSecond += randomInt(random, 12, 45);
    }

    let errorsThisSecond = Math.round(
      handlersThisSecond
      * (
        profile.errorFloor
        + (incident.errors * profile.errorIncidentScale)
        + (Math.max(0, load - 0.97) * profile.errorIncidentScale * 0.35)
      )
    );

    if (incident.errors > 0.2 && random() < 0.12) {
      errorsThisSecond += randomInt(random, 1, 3);
    }

    errorsThisSecond = clamp(errorsThisSecond, 0, Math.max(1, Math.floor(handlersThisSecond * 0.025)));

    const receiveBytesThisSecond = Math.round(
      (handlersThisSecond * profile.receiveBytesPerHandler * (0.92 + (random() * 0.17)))
      + (activeConnections * profile.receiveOverheadPerConnection * (0.88 + (random() * 0.18)))
    );

    const sendBytesThisSecond = Math.round(
      (handlersThisSecond * profile.sendBytesPerHandler * (0.9 + (random() * 0.2)))
      + (activeConnections * profile.sendOverheadPerConnection * (0.9 + (random() * 0.2)))
    );

    totalHandlers += handlersThisSecond;
    totalErrors += errorsThisSecond;
    bytesSent += sendBytesThisSecond;
    bytesReceived += receiveBytesThisSecond;

    const timestamp = new Date(startTime.getTime() + (second * 1000));
    timeseries[second] = {
      timestamp: timestamp.toISOString(),
      sequenceNumber: second + 1,
      uptimeSeconds: profile.uptimeBaseSeconds + second,
      handlersExecutedPerSecond: round(handlerRateWindow.push(handlersThisSecond), 2),
      handlerErrorsPerSecond: round(errorRateWindow.push(errorsThisSecond), 2),
      totalHandlersExecuted: totalHandlers,
      totalHandlerErrors: totalErrors,
      activeConnections,
      peakActiveConnections,
      acceptedConnections,
      rejectedConnections,
      disconnectedConnections,
      timedOutConnections,
      bytesSent,
      bytesReceived,
      sendBytesPerSecond: round(sendRateWindow.push(sendBytesThisSecond), 2),
      receiveBytesPerSecond: round(receiveRateWindow.push(receiveBytesThisSecond), 2)
    };
  }

  const handlers = buildHandlerRows(profile.handlers, totalHandlers, totalErrors, random);
  const durationHistogram = buildHistogram(
    totalHandlers * profile.histogramMultipliers.duration,
    profile.histogramWeights.duration
  );
  const parseHistogram = buildHistogram(
    totalHandlers * profile.histogramMultipliers.parse,
    profile.histogramWeights.parse
  );
  const queueDelayHistogram = buildHistogram(
    totalHandlers * profile.histogramMultipliers.queueDelay,
    profile.histogramWeights.queueDelay
  );
  const receivedHandlerDurationHistogram = buildHistogram(
    totalHandlers * profile.histogramMultipliers.receivedHandlerDuration,
    profile.histogramWeights.receivedHandlerDuration
  );

  const payloads = {
    "timeseries.json": JSON.stringify(timeseries),
    "handlers.json": JSON.stringify(handlers),
    "duration_histogram.json": JSON.stringify(durationHistogram),
    "parse_histogram.json": JSON.stringify(parseHistogram),
    "queue_delay_histogram.json": JSON.stringify(queueDelayHistogram),
    "received_handler_duration_histogram.json": JSON.stringify(receivedHandlerDurationHistogram)
  };

  return {
    payloads,
    summary: {
      server: serverName,
      points: timeseries.length,
      totalHandlers,
      totalErrors,
      bytesSent,
      bytesReceived
    }
  };
}

function buildHandlerRows(handlerDefinitions, totalHandlers, totalErrors, random) {
  const adjustedShares = handlerDefinitions.map((handler) => handler.share * (0.96 + (random() * 0.08)));
  const executionCounts = allocateCounts(totalHandlers, adjustedShares);
  const errorWeights = handlerDefinitions.map((handler, index) =>
    Math.max(0.000001, handler.errorWeight * executionCounts[index])
  );
  const errorCounts = allocateCounts(totalErrors, errorWeights);

  return handlerDefinitions.map((handler, index) => {
    const avgDurationMs = round(handler.avgDurationMs * (0.94 + (random() * 0.12)), 3);
    const minDurationMs = round(
      Math.max(0.002, avgDurationMs * handler.minFactor * (0.85 + (random() * 0.3))),
      3
    );
    const maxDurationMs = round(
      Math.max(avgDurationMs * handler.maxFactor * (0.9 + (random() * 0.35)), minDurationMs + 0.5),
      1
    );

    return {
      handlerName: handler.name,
      executionCount: executionCounts[index],
      errorCount: errorCounts[index],
      avgDurationMs,
      minDurationMs,
      maxDurationMs
    };
  }).sort((left, right) => right.executionCount - left.executionCount);
}

function buildHistogram(total, weights) {
  const bucketCounts = allocateCounts(Math.max(DURATION_BUCKETS.length, Math.round(total)), weights);
  return DURATION_BUCKETS.map((bucket, index) => ({
    bucket,
    count: bucketCounts[index]
  }));
}

function allocateCounts(total, weights) {
  const safeTotal = Math.max(0, Math.round(total));
  const weightSum = weights.reduce((sum, value) => sum + value, 0);
  if (weightSum <= 0 || safeTotal === 0) {
    return weights.map(() => 0);
  }

  const raw = weights.map((weight) => (safeTotal * weight) / weightSum);
  const counts = raw.map((value) => Math.floor(value));
  let remaining = safeTotal - counts.reduce((sum, value) => sum + value, 0);

  const rankedRemainders = raw
    .map((value, index) => ({ index, remainder: value - Math.floor(value) }))
    .sort((left, right) => right.remainder - left.remainder);

  for (let index = 0; index < remaining; index++) {
    counts[rankedRemainders[index % rankedRemainders.length].index] += 1;
  }

  return counts;
}

class RollingAverage {
  constructor(size) {
    this.size = Math.max(1, size);
    this.values = [];
    this.sum = 0;
  }

  push(value) {
    this.values.push(value);
    this.sum += value;

    if (this.values.length > this.size) {
      this.sum -= this.values.shift();
    }

    return this.sum / this.values.length;
  }
}

function computeLoad(second, profile) {
  const position = second / SECONDS_PER_DAY;
  const primary = (Math.sin((position * Math.PI * 2) - profile.dayPhase) + 1) / 2;
  const secondary = (Math.sin((position * Math.PI * 4) + profile.secondaryPhase) + 1) / 2;
  const evening = gaussianPulse(second, profile.eveningPeakCenter, profile.eveningPeakWidth, profile.eveningPeakBoost);
  const maintenance = gaussianPulse(second, profile.maintenanceCenter, profile.maintenanceWidth, -profile.maintenanceDrop);
  return clamp(0.18 + (primary * 0.5) + (secondary * 0.12) + evening + maintenance, 0.06, 1.28);
}

function computeIncidentEffects(second, incidents) {
  let activity = 0;
  let connectionOffset = 0;
  let latency = 0;
  let errors = 0;
  let rejections = 0;
  let timeouts = 0;

  for (const incident of incidents) {
    const pulse = gaussianPulse(second, incident.center, incident.width, 1);
    activity += pulse * incident.activity;
    connectionOffset += pulse * incident.connectionOffset;
    latency += pulse * incident.latency;
    errors += pulse * incident.errors;
    rejections += pulse * incident.rejections;
    timeouts += pulse * incident.timeouts;
  }

  return {
    activity,
    connectionOffset,
    latency,
    errors,
    rejections,
    timeouts
  };
}

function gaussianPulse(value, center, width, amplitude) {
  const distance = (value - center) / width;
  return amplitude * Math.exp(-(distance * distance) * 0.5);
}

function floorToSecond(date) {
  return new Date(Math.floor(date.getTime() / 1000) * 1000);
}

function formatBytes(bytes) {
  if (bytes >= 1024 ** 4) {
    return `${round(bytes / (1024 ** 4), 2)} TB`;
  }
  if (bytes >= 1024 ** 3) {
    return `${round(bytes / (1024 ** 3), 2)} GB`;
  }
  if (bytes >= 1024 ** 2) {
    return `${round(bytes / (1024 ** 2), 2)} MB`;
  }
  if (bytes >= 1024) {
    return `${round(bytes / 1024, 2)} KB`;
  }
  return `${bytes} B`;
}

function getOutputRoots(explicitOutputDir) {
  const dashboardRoot = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");

  if (explicitOutputDir) {
    return [path.resolve(process.cwd(), explicitOutputDir)];
  }

  return [
    path.join(dashboardRoot, "src", "metrics"),
    path.join(dashboardRoot, "dist", "metrics")
  ];
}

async function writeDataset(outputRoots, serverName, payloads) {
  for (const outputRoot of outputRoots) {
    const serverDir = path.join(outputRoot, serverName);
    await fs.mkdir(serverDir, { recursive: true });

    for (const [fileName, content] of Object.entries(payloads)) {
      await fs.writeFile(path.join(serverDir, fileName), `${content}\n`, "utf8");
    }
  }
}

function parseArgs(args) {
  const options = {};

  for (let index = 0; index < args.length; index++) {
    const argument = args[index];
    if (!argument.startsWith("--")) {
      throw new Error(`Unexpected argument: ${argument}`);
    }

    const [rawKey, inlineValue] = argument.slice(2).split("=", 2);
    if (inlineValue !== undefined) {
      options[rawKey] = inlineValue;
      continue;
    }

    const nextValue = args[index + 1];
    if (nextValue && !nextValue.startsWith("--")) {
      options[rawKey] = nextValue;
      index += 1;
    } else {
      options[rawKey] = true;
    }
  }

  return options;
}

function parseIntegerOption(value, fallback, name) {
  if (value === undefined) {
    return fallback;
  }

  const parsed = Number.parseInt(value, 10);
  if (!Number.isFinite(parsed)) {
    throw new Error(`Invalid --${name} value: ${value}`);
  }

  return parsed;
}

function randomCentered(random, magnitude) {
  return (random() - 0.5) * 2 * magnitude;
}

function randomInt(random, min, max) {
  return Math.floor(random() * (max - min + 1)) + min;
}

function round(value, digits) {
  const scale = 10 ** digits;
  return Math.round(value * scale) / scale;
}

function clamp(value, min, max) {
  return Math.min(max, Math.max(min, value));
}

function mulberry32(seed) {
  let state = seed >>> 0;
  return function next() {
    state = (state + 0x6d2b79f5) >>> 0;
    let result = Math.imul(state ^ (state >>> 15), 1 | state);
    result ^= result + Math.imul(result ^ (result >>> 7), 61 | result);
    return ((result ^ (result >>> 14)) >>> 0) / 4294967296;
  };
}

function printHelp() {
  console.log(`Generate 24 hours of 1-second dummy metrics for the DDON dashboard.

Usage:
  npm run generate:dummy-data
  npm run generate:dummy-data -- --output-dir /tmp/ddon-metrics
  npm run generate:dummy-data -- --seed 1234 --end 2026-03-19T23:59:59Z

Options:
  --output-dir <path>  Write metrics to a custom root directory instead of src/dist.
  --seed <number>      Override the deterministic random seed.
  --end <iso-date>     End timestamp for the generated 24-hour window.
  --help               Show this message.
`);
}

main().catch((error) => {
  console.error(error instanceof Error ? error.message : String(error));
  process.exitCode = 1;
});
