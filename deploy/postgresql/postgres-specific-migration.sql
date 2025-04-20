-- Intended to be run after the DB/app has started up

-- Enable useful extensions

create extension if not exists pg_stat_statements;
create extension if not exists pg_buffercache;
create extension if not exists pg_trgm;

-- PSQL-specific indexes

CREATE INDEX concurrently IF NOT EXISTS "idx_ddon_character_first_name_trgm" ON "ddon_character" USING GIN (LOWER("first_name") gin_trgm_ops);
CREATE INDEX concurrently IF NOT EXISTS "idx_ddon_character_last_name_trgm" ON "ddon_character" USING GIN (LOWER("last_name") gin_trgm_ops);
CREATE index concurrently IF NOT EXISTS "idx_ddon_pawn_lower_name_trgm" ON "ddon_pawn" USING GIN (LOWER("name") gin_trgm_ops);

--- Custom statistics width

ALTER TABLE "ddon_storage_item" ALTER COLUMN "character_id" SET STATISTICS 1000;

-- Vacuum

vacuum (full, freeze, verbose, analyze);
