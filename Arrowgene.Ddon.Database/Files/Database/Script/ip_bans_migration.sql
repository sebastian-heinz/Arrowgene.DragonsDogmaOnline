CREATE TABLE IF NOT EXISTS "account_ip_ban"
(
    "addr"                TEXT      PRIMARY KEY NOT NULL,
    "date"                DATETIME              NOT NULL DEFAULT (CURRENT_TIMESTAMP)
);
