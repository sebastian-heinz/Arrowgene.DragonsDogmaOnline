-- Note: This migration will fail if you have duplicates in your DB!
-- What follows are no-warranty duplicate cleaners that delete one of the duplicate rows without applying any criteria:

-- PostgreSQL
--WITH marked AS (SELECT ctid, row_number() over (partition by character_id, job_id, release_type, release_id, target_id ORDER BY ctid) AS rn FROM ddon_job_master_active_orders_progress)
--delete FROM ddon_job_master_active_orders_progress q using marked m
--WHERE q.ctid = m.ctid and m.rn > 1;
-- SQLite
--WITH marked AS (SELECT rowid AS rid, ROW_NUMBER() OVER (PARTITION BY character_id, job_id, release_type, release_id, target_id ORDER BY rowid) AS rn FROM ddon_job_master_active_orders_progress)
--DELETE FROM ddon_job_master_active_orders_progress
--WHERE rowid IN (SELECT rid FROM marked WHERE rn > 1);

CREATE TABLE IF NOT EXISTS "ddon_job_master_active_orders_progress_new"
(
    "character_id" INTEGER NOT NULL,
    "job_id"       INTEGER NOT NULL,
    "release_type" INTEGER NOT NULL,
    "release_id"   INTEGER NOT NULL,
    "target_id"    INTEGER NOT NULL,
    "condition"    INTEGER NOT NULL,
    "target_rank"  INTEGER NOT NULL,
    "target_num"   INTEGER NOT NULL,
    "current_num"  INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_job_master_active_orders_progress" PRIMARY KEY ("character_id", "job_id", "release_type", "release_id", "target_id"),
    CONSTRAINT "fk_ddon_job_master_active_orders_progress" FOREIGN KEY ("character_id", "job_id", "release_type", "release_id") REFERENCES "ddon_job_master_active_orders" ("character_id", "job_id", "release_type", "release_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_job_master_active_orders_progress_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
INSERT INTO "ddon_job_master_active_orders_progress_new" SELECT "character_id", "job_id", "release_type", "release_id", "target_id", "condition", "target_rank", "target_num", "current_num" FROM "ddon_job_master_active_orders_progress";
DROP TABLE "ddon_job_master_active_orders_progress";
ALTER TABLE "ddon_job_master_active_orders_progress_new" RENAME TO "ddon_job_master_active_orders_progress";
