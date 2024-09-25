INSERT INTO ddon_quest_progress(character_common_id, quest_type, quest_id, step, variant_quest_id)
VALUES ((SELECT character_common_id FROM ddon_completed_quests WHERE quest_id = 20200), 3, 20210, 0, 0);
