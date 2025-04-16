CREATE TABLE ddon_job_master_released_elements (
	"character_id"	INTEGER NOT NULL,
	"job_id"	    INTEGER NOT NULL,
    "release_type"  INTEGER NOT NULL,
	"release_id"	INTEGER NOT NULL,
    "release_level" INTEGER NOT NULL,
    CONSTRAINT pk_ddon_job_master_released_elements PRIMARY KEY ("character_id", "job_id", "release_type", "release_id", "release_level"),
    CONSTRAINT fk_ddon_job_master_released_elements_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE ddon_job_master_active_orders (
	"character_id"	INTEGER NOT NULL,
	"job_id"	    INTEGER NOT NULL,
    "release_type"  INTEGER NOT NULL,
	"release_id"	INTEGER NOT NULL,
	"release_level"	INTEGER NOT NULL,
    "order_accepted" BOOLEAN NOT NULL,
    CONSTRAINT pk_ddon_job_master_active_orders PRIMARY KEY ("character_id", "job_id", "release_type", "release_id"),
    CONSTRAINT fk_ddon_job_master_active_orders_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE ddon_job_master_active_orders_progress (
	"character_id"	INTEGER NOT NULL,
	"job_id"	    INTEGER NOT NULL,
    "release_type"  INTEGER NOT NULL,
	"release_id"	INTEGER NOT NULL,
    "condition"	    INTEGER NOT NULL,
	"target_id"	    INTEGER NOT NULL,
    "target_rank"	INTEGER NOT NULL,
	"target_num"	INTEGER NOT NULL,
    "current_num"	INTEGER NOT NULL,
    CONSTRAINT fk_ddon_job_master_active_orders_progress FOREIGN KEY ("character_id", "job_id", "release_type", "release_id") REFERENCES "ddon_job_master_active_orders" ("character_id", "job_id", "release_type", "release_id") ON DELETE CASCADE,
    CONSTRAINT fk_ddon_job_master_active_orders_progress_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
