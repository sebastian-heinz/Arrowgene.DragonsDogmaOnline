#!/usr/bin/env node

import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const dashboardRoot = path.resolve(__dirname, "..");
const sourceDir = path.join(dashboardRoot, "dist");
const targetDir = path.resolve(
  dashboardRoot,
  "../../Arrowgene.Ddon.WebServer/Files/www/metrics"
);

function shouldCopy(sourcePath) {
  const relativePath = path.relative(sourceDir, sourcePath);
  return relativePath !== "snapshot" && !relativePath.startsWith(`snapshot${path.sep}`);
}

await fs.rm(targetDir, { recursive: true, force: true });
await fs.mkdir(path.dirname(targetDir), { recursive: true });
await fs.cp(sourceDir, targetDir, { recursive: true, filter: shouldCopy });

process.stdout.write(`Copied ${sourceDir} -> ${targetDir}\n`);
