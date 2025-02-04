CREATE TABLE "ddon_clan_shop_purchases"
(
    "clan_id"               INTEGER     NOT NULL,
    "lineup_id"             INTEGER     NOT NULL,
    CONSTRAINT "pk_ddon_clan_shop_purchases" PRIMARY KEY ("clan_id", "lineup_id"),
    CONSTRAINT "fl_ddon_clan_shop_purchases_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_clan_base_customization"
(
    "clan_id"               INTEGER     NOT NULL,
    "type"                  INTEGER     NOT NULL,
    "furniture_id"          INTEGER     NOT NULL,
    CONSTRAINT "pk_ddon_clan_base_customization" PRIMARY KEY ("clan_id", "type"),
    CONSTRAINT "fl_ddon_clan_base_customization_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE
);
