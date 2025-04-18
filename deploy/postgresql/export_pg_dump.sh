#!/bin/bash
# This script uses pg_dump inside a Postgres Docker container to export a database dump to the host filesystem.
# It creates a temporary .pgpass file inside the container for password authentication.
#
# Usage:
#   ./export_pg_dump.sh <CONTAINER_NAME> <DATABASE_NAME> <USERNAME> <DB_PASSWORD> [OUTPUT_DIR]
#
# Example:
#   ./export_pg_dump.sh ddon-psql postgres postgres postgres ./

if [ "$#" -lt 4 ]; then
  echo "Usage: $0 <CONTAINER_NAME> <DATABASE_NAME> <USERNAME> <DB_PASSWORD> [OUTPUT_DIR]"
  exit 1
fi

CONTAINER_NAME=$1
DATABASE_NAME=$2
DB_USER=$3
DB_PASSWORD=$4
OUTPUT_DIR=${5:-$(pwd)}  # Default to current directory if not provided

# Generate a timestamp and an output file name
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")
mkdir -p "${OUTPUT_DIR}"
OUTPUT_FILE="${OUTPUT_DIR}/${DATABASE_NAME}_${TIMESTAMP}.sql"

echo "Starting backup of '${DATABASE_NAME}' from container '${CONTAINER_NAME}'..."
echo "Backup will be stored in: ${OUTPUT_FILE}"

# Define the .pgpass file content
# Assuming the dump command connects using the default host and port.
# If your container uses a different host or port, adjust the entry accordingly.
PGPASS_CONTENT="localhost:5432:${DATABASE_NAME}:${DB_USER}:${DB_PASSWORD}"

# Create a temporary .pgpass file inside the container, run the dump, and then remove the file.
# Using bash -c in docker exec so we can chain commands.
docker exec "${CONTAINER_NAME}" bash -c "\
  echo '${PGPASS_CONTENT}' > ~/.pgpass && \
  chmod 600 ~/.pgpass && \
  pg_dump -Fc -U ${DB_USER} ${DATABASE_NAME} && \
  rm ~/.pgpass" > "${OUTPUT_FILE}"
EXIT_CODE=$?

if [ ${EXIT_CODE} -ne 0 ]; then
  echo "Error: pg_dump failed with exit code ${EXIT_CODE}."
  exit ${EXIT_CODE}
fi

echo "Backup completed successfully and saved to ${OUTPUT_FILE}."
