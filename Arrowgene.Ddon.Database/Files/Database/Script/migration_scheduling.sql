CREATE TABLE ddon_schedule_next (
	"type"	INTEGER NOT NULL,
	"timestamp"	BIGINT NOT NULL,
	PRIMARY KEY("type")
);

INSERT INTO ddon_schedule_next(type, timestamp) VALUES (19, 0);
