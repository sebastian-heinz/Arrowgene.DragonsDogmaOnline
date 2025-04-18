#!/bin/bash
# This script restores a Postgres database from a dump file using pg_restore,
# while running inside a Docker container.
#
# Usage:
#   ./import_pg_dump.sh <CONTAINER_NAME> <DATABASE_NAME> <USERNAME> <DB_PASSWORD> <DUMP_FILE_PATH>
#
# Example:
#   ./import_pg_dump.sh ddon-psql postgres postgres postgres ./postgres.sql
#
# Note:
# - The dump file must be in the pg_dump custom format (-Fc).
# - The script uses a temporary .pgpass file inside the container for password authentication.

# Verify required arguments
if [ "$#" -ne 5 ]; then
  echo "Usage: $0 <CONTAINER_NAME> <DATABASE_NAME> <USERNAME> <DB_PASSWORD> <DUMP_FILE_PATH>"
  exit 1
fi

CONTAINER_NAME=$1
DATABASE_NAME=$2
DB_USER=$3
DB_PASSWORD=$4
DUMP_FILE=$5

# Check if dump file exists on host
if [ ! -f "$DUMP_FILE" ]; then
  echo "Error: Dump file '$DUMP_FILE' does not exist."
  exit 1
fi

echo "Copying dump file '$DUMP_FILE' into container '$CONTAINER_NAME'..."
# Copy the dump file to the container's /tmp directory
docker cp "$DUMP_FILE" "${CONTAINER_NAME}:/tmp/restore.dump"
if [ $? -ne 0 ]; then
  echo "Error: Failed to copy dump file to container."
  exit 1
fi

echo "Starting restore for database '${DATABASE_NAME}' inside container '${CONTAINER_NAME}'..."

# Define the .pgpass entry for authentication
PGPASS_ENTRY="localhost:5432:${DATABASE_NAME}:${DB_USER}:${DB_PASSWORD}"

# Execute restore inside the container:
# 1. Create a temporary .pgpass file in the home directory,
# 2. Set secure permissions,
# 3. Run pg_restore with --clean and --if-exists options to drop existing objects,
# 4. Remove the temporary .pgpass file.
docker exec "${CONTAINER_NAME}" bash -c "\
  echo '${PGPASS_ENTRY}' > ~/.pgpass && \
  chmod 600 ~/.pgpass && \
  pg_restore -U ${DB_USER} -d ${DATABASE_NAME} --clean --if-exists -v /tmp/restore.dump && \
  rm ~/.pgpass"
EXIT_CODE=$?

if [ ${EXIT_CODE} -ne 0 ]; then
  echo "Error: pg_restore failed with exit code ${EXIT_CODE}."
  docker exec "${CONTAINER_NAME}" rm -f /tmp/restore.dump
  exit ${EXIT_CODE}
fi

docker exec "${CONTAINER_NAME}" rm -f /tmp/restore.dump

echo "Database restore completed successfully."
