import { metricFileNames, servers } from "./src/metrics-config.js";

export default {
  title: "DDON Server Metrics",
  root: "src",
  theme: ["dashboard", "near-midnight", "wide"],
  dynamicPaths: servers.flatMap((server) =>
    Object.values(metricFileNames).map((fileName) => `/snapshot/${server}/${fileName}`)
  ),
};
