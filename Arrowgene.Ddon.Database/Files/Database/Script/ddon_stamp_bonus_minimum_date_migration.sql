UPDATE ddon_stamp_bonus
SET last_stamp_time = '1900-01-01 00:00:00+00'
WHERE last_stamp_time < '1900-01-01';
