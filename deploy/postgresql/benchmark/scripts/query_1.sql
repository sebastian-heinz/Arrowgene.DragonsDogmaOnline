SELECT clan_id,
       "name",
       clan_level,
       member_num,
       motto,
       emblem_mark_type,
       emblem_base_type,
       emblem_main_color,
       emblem_sub_color,
       master_id,
       created
FROM ddon_clan_param
WHERE "name" LIKE '%a%'
  AND clan_level = 2
  --AND member_num = 5
  --AND (motto & 3) > 0
  AND (active_days & 10) > 0
  AND (active_time & 2) > 0
  AND (characteristic & 3) > 0
LIMIT 250;
