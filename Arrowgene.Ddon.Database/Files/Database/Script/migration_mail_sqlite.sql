CREATE TABLE "ddon_system_mail" (
	"message_id"	INTEGER NOT NULL,
	"character_id"	INTEGER NOT NULL,
	"message_state"	INTEGER NOT NULL,
	"sender_name"	VARCHAR(256) NOT NULL DEFAULT "",
	"message_title"	VARCHAR(256) NOT NULL DEFAULT "",
	"message_body"	VARCHAR(2048) NOT NULL DEFAULT "",
	"send_date"	INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE,
	PRIMARY KEY("message_id" AUTOINCREMENT)
);

CREATE TABLE "ddon_system_mail_attachment" (
	"message_id"	INTEGER NOT NULL,
	"attachment_id"	INTEGER NOT NULL,
	"attachment_type"	INTEGER NOT NULL,
	"is_received"	BOOLEAN NOT NULL DEFAULT FALSE,
	"param0"	VARCHAR(256) NOT NULL DEFAULT "",
	"param1"	INTEGER NOT NULL DEFAULT 0,
	"param2"	INTEGER NOT NULL DEFAULT 0,
	"param3"	INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY("message_id") REFERENCES "ddon_system_mail"("message_id") ON DELETE CASCADE
);
