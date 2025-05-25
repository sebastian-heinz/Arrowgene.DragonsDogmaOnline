#!/usr/bin/env bash
set -euo pipefail

SCALES=(1000 10000 100000)
CLIENTS=(1)
RESULTS_DIR=/scripts/results
mkdir -p "$RESULTS_DIR"

for scale in "${SCALES[@]}"; do
  psql -t -q -A -h db -U bench -d benchmark -f /scripts/create_table.sql
  #psql -h db -U bench -d benchmark -v rows="$scale" -f /scripts/data_gen.sql
  ROWS=$scale python3 /scripts/data_gen.py
  psql -t -q -A -h db -U bench -d benchmark -c "vacuum (full, freeze, verbose, analyze);"
  
  for c in "${CLIENTS[@]}"; do
    label="scale_${scale}_clients_${c}"
    
    pgbench -h db -U bench -d benchmark -n -c "$c" -T 5 -f /scripts/query_1.sql | tee "$RESULTS_DIR/pgbench_query1_${label}.log"
    pgbench -h db -U bench -d benchmark -n -c "$c" -T 5 -f /scripts/query_2.sql | tee "$RESULTS_DIR/pgbench_query2_${label}.log"
  done
  
  query1=$(cat /scripts/query_1.sql)
  query2=$(cat /scripts/query_2.sql)
  psql -t -q -A -h db -U bench -d benchmark -c "EXPLAIN (ANALYZE, VERBOSE, COSTS, SETTINGS, BUFFERS, WAL, TIMING, SUMMARY, MEMORY, FORMAT JSON) ${query1}" -o "$RESULTS_DIR/plan_query1_${scale}.json"
  psql -t -q -A -h db -U bench -d benchmark -c "EXPLAIN (ANALYZE, VERBOSE, COSTS, SETTINGS, BUFFERS, WAL, TIMING, SUMMARY, MEMORY, FORMAT JSON) ${query2}" -o "$RESULTS_DIR/plan_query2_${scale}.json"
done
