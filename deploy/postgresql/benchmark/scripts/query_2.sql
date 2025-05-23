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
WHERE (false OR ("name" LIKE '%a%'))
  AND (false OR (clan_level = 2))
  AND (true OR (member_num = 5))
  AND (true OR (motto & 3 > 0))
  AND (false OR (active_days & 10 > 0))
  AND (false OR (active_time & 2 > 0))
  AND (false OR (characteristic & 3 > 0))
LIMIT 250;
