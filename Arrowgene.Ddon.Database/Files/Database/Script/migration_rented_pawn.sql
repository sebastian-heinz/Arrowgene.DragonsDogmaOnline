ALTER TABLE "ddon_pawn" ADD "is_official_pawn" INTEGER DEFAULT 0 NOT NULL;

UPDATE ddon_character SET rental_pawn_slot_num=3 WHERE my_pawn_slot_num > 0 AND game_mode = 1;
