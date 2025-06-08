INSERT INTO ddon_quest_progress(character_common_id, quest_type, quest_schedule_id, step)
SELECT ddon_completed_quests.character_common_id, 3, 6292736, 0
FROM ddon_completed_quests
WHERE ddon_completed_quests.quest_id = 20250;
