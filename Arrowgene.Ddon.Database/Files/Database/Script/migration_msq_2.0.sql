INSERT INTO ddon_quest_progress(character_common_id, quest_type, quest_id, step, variant_quest_id)
SELECT ddon_completed_quests.character_common_id, 3, 20010, 0, 0
FROM ddon_completed_quests
WHERE ddon_completed_quests.quest_id = 30;
