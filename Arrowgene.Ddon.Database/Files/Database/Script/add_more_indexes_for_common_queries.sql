CREATE INDEX IF NOT EXISTS "idx_ddon_contact_list_requester_character_id" ON "ddon_contact_list" ("requester_character_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_contact_list_requested_character_id" ON "ddon_contact_list" ("requested_character_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_bazaar_exhibition_all_filter" ON "ddon_bazaar_exhibition" ("item_id", "state", "expire", "character_id", "price");
CREATE INDEX IF NOT EXISTS "idx_ddon_bazaar_exhibition_state_expire" ON "ddon_bazaar_exhibition" ("state", "expire");
CREATE INDEX IF NOT EXISTS "idx_ddon_reward_box_character_common_id" ON "ddon_reward_box" ("character_common_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_completed_quests_character_common_id_quest_id" ON "ddon_completed_quests" ("character_common_id", "quest_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_system_mail_attachment_message_id" ON "ddon_system_mail_attachment"("message_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_crests_item_uid" ON "ddon_crests"("item_uid");
CREATE INDEX IF NOT EXISTS "idx_ddon_equipment_limit_break_item_uid" ON "ddon_equipment_limit_break" ("item_uid");

ANALYZE;
