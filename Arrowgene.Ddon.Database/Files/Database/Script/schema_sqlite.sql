CREATE TABLE IF NOT EXISTS `setting`
(
    `key`   TEXT NOT NULL,
    `value` TEXT NOT NULL,
    PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account`
(
    `id`                  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `name`                TEXT                              NOT NULL,
    `normal_name`         TEXT                              NOT NULL,
    `hash`                TEXT                              NOT NULL,
    `mail`                TEXT                              NOT NULL,
    `mail_verified`       INTEGER                           NOT NULL,
    `mail_verified_at`    DATETIME DEFAULT NULL,
    `mail_token`          TEXT     DEFAULT NULL,
    `password_token`      TEXT     DEFAULT NULL,
    `login_token`         TEXT     DEFAULT NULL,
    `login_token_created` DATETIME                          NOT NULL,
    `state`               INTEGER                           NOT NULL,
    `last_login`          DATETIME DEFAULT NULL,
    `created`             DATETIME                          NOT NULL,
    CONSTRAINT `uq_account_name` UNIQUE (`name`),
    CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
    CONSTRAINT `uq_account_login_token` UNIQUE (`login_token`),
    CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);

CREATE TABLE IF NOT EXISTS `ddo_character`
(
    `id`         INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `account_id` INTEGER                           NOT NULL,
    `first_name` TEXT                              NOT NULL,
    `last_name`  TEXT                              NOT NULL,
    `created`    DATETIME                          NOT NULL,
    CONSTRAINT `fk_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
);

CREATE TABLE IF NOT EXISTS `ddo_game_token`
(
    `account_id`   INTEGER PRIMARY KEY NOT NULL,
    `character_id` INTEGER             NOT NULL,
    `token`        TEXT                NOT NULL,
    `created`      DATETIME            NOT NULL,
    CONSTRAINT `uq_game_token_token` UNIQUE (`token`),
    CONSTRAINT `fk_game_token_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
    CONSTRAINT `fk_game_token_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddo_character` (`id`)
);