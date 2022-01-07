CREATE TABLE IF NOT EXISTS `setting` (
  `key`   TEXT NOT NULL,
  `value` TEXT NOT NULL,
  PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  `name`             TEXT                              NOT NULL,
  `normal_name`      TEXT                              NOT NULL,
  `hash`             TEXT                              NOT NULL,
  `mail`             TEXT                              NOT NULL,
  `mail_verified`    INTEGER                           NOT NULL,
  `mail_verified_at` DATETIME                                    DEFAULT NULL,
  `mail_token`       TEXT                                        DEFAULT NULL,
  `password_token`   TEXT                                        DEFAULT NULL,
  `state`            INTEGER                           NOT NULL,
  `last_login`       DATETIME                                    DEFAULT NULL,
  `created`          DATETIME                          NOT NULL,
  CONSTRAINT `uq_account_name` UNIQUE (`name`),
  CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
  CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);