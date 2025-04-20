#!/bin/bash
# Converts SQLite schema to target database format and creates type-specific file

# Configuration
REPO_ROOT="../Arrowgene.Ddon.Database"
SCHEMA_DIR="${REPO_ROOT}/Files/Database/Script"
INPUT_SCHEMA="${SCHEMA_DIR}/schema_sqlite.sql"

# Validate environment
if [ ! -d "$SCHEMA_DIR" ]; then
    echo "Error: Schema directory not found: $SCHEMA_DIR"
    exit 1
fi

if [ ! -f "$INPUT_SCHEMA" ]; then
    echo "Error: Source schema not found: $INPUT_SCHEMA"
    exit 1
fi

# Function to show usage
usage() {
    echo "Usage: $0 <database_type>"
    echo "Supported types: postgres, mariadb"
    exit 1
}

# Validate arguments
if [ $# -ne 1 ]; then
    usage
fi

DB_TYPE=$(echo "$1" | tr '[:upper:]' '[:lower:]')
OUTPUT_SCHEMA="${SCHEMA_DIR}/schema_${DB_TYPE}.sql"

# Check for existing file
if [ -f "$OUTPUT_SCHEMA" ]; then
    echo "Warning: Output file exists - overwriting: $OUTPUT_SCHEMA"
fi

# Perform conversions
case $DB_TYPE in
    postgres|postgresql)
        sed -E "
            s/TINYINT/SMALLINT/gi;
            s/([[:space:]])DATETIME([[:space:],])/\1TIMESTAMP WITH TIME ZONE\2/gi;
            s/([[:space:]])INTEGER[[:space:]]+PRIMARY[[:space:]]+KEY[[:space:]]+AUTOINCREMENT([[:space:],])/\1SERIAL PRIMARY KEY\2/gi;
            s/([[:space:]])BLOB([[:space:],])/\1BYTEA\2/gi;
            s/PRAGMA[[:space:]]+foreign_keys[[:space:]]*=[[:space:]]*(OFF|0)[[:space:]]*;/SET session_replication_role='replica';/gi;
            s/PRAGMA[[:space:]]+foreign_keys[[:space:]]*=[[:space:]]*(ON|1)[[:space:]]*;/SET session_replication_role='origin';/gi;
        " "$INPUT_SCHEMA" > "$OUTPUT_SCHEMA"
        ;;
    mariadb|mysql)
        sed -E "
            s/([[:space:]])AUTOINCREMENT([[:space:],])/\1AUTO_INCREMENT\2/gi;
            s/PRAGMA[[:space:]]+foreign_keys[[:space:]]*=[[:space:]]*(OFF|0)[[:space:]]*;/SET FOREIGN_KEY_CHECKS=0;/gi;
            s/PRAGMA[[:space:]]+foreign_keys[[:space:]]*=[[:space:]]*(ON|1)[[:space:]]*;/SET FOREIGN_KEY_CHECKS=1;/gi;
        " "$INPUT_SCHEMA" > "$OUTPUT_SCHEMA"
        ;;
    *)
        echo "Error: Unsupported database type: $DB_TYPE"
        usage
        ;;
esac

# Report results
if [ $? -eq 0 ]; then
    echo "Generated adapted schema for ${DB_TYPE}:"
    echo "  Input:  ${INPUT_SCHEMA}"
    echo "  Output: ${OUTPUT_SCHEMA}"
else
    echo "Error: Schema conversion failed"
    exit 1
fi