CREATE TABLE "ddon_system_mail_attachment" (
    "attachment_id"	INTEGER NOT NULL,
	"message_id"	INTEGER NOT NULL,
	"attachment_type"	INTEGER NOT NULL,
	"is_received"	INTEGER NOT NULL DEFAULT 0,
	"param0"	VARCHAR(256) NOT NULL DEFAULT "",
	"param1"	INTEGER NOT NULL DEFAULT 0,
	"param2"	INTEGER NOT NULL DEFAULT 0,
	"param3"	INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY("attachment_id" AUTOINCREMENT),
	FOREIGN KEY("message_id") REFERENCES "ddon_system_mail"("message_id") ON DELETE CASCADE
);