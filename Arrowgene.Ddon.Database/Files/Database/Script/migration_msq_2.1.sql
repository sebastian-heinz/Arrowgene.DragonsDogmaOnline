INSERT INTO ddon_quest_progress(character_common_id, quest_type, quest_id, step, variant_quest_id)
VALUES ((SELECT character_common_id FROM ddon_completed_quests WHERE quest_id = 20070), 3, 20080, 0, 0);
