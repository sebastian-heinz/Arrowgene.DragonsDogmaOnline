CREATE TABLE IF NOT EXISTS `setting` (
  `key`   VARCHAR(255) NOT NULL,
  `value` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`key`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INT(11)      NOT NULL AUTO_INCREMENT,
  `name`             VARCHAR(17)  NOT NULL,
  `normal_name`      VARCHAR(17)  NOT NULL,
  `hash`             VARCHAR(255) NOT NULL,
  `mail`             VARCHAR(255) NOT NULL,
  `mail_verified`    TINYINT(1)   NOT NULL,
  `mail_verified_at` DATETIME               DEFAULT NULL,
  `mail_token`       VARCHAR(255)           DEFAULT NULL,
  `password_token`   VARCHAR(255)           DEFAULT NULL,
  `state`            INT(11)      NOT NULL,
  `last_login`       DATETIME               DEFAULT NULL,
  `created`          DATETIME     NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_account_name` (`name`),
  UNIQUE KEY `uq_account_normal_name` (`normal_name`),
  UNIQUE KEY `uq_account_mail` (`mail`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;