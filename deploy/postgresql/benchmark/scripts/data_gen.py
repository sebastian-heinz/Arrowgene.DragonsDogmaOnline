#!/usr/bin/env python3
import numpy as np
import os
import psycopg2
import random
import string
import time
from datetime import datetime, timedelta
from faker import Faker
from psycopg2.extras import execute_values

ROWS = int(os.getenv("ROWS", "1000000"))
BATCH_SIZE = int(os.getenv("BATCH_SIZE", "1000000"))
SEED = int(os.getenv("SEED", "42"))

# Initialize RNGs
faker = Faker()
Faker.seed(SEED)
rng = np.random.default_rng(SEED)

# PostgreSQL connection
conn = psycopg2.connect(dbname="benchmark", user="bench", password="bench", host="db")
cur = conn.cursor()
cur.execute("TRUNCATE ddon_clan_param;")
conn.commit()

insert_sql = """
             INSERT INTO ddon_clan_param (clan_level, member_num, master_id, system_restriction, is_base_release,
                                          can_base_release, total_clan_point, money_clan_point, name, short_name,
                                          emblem_mark_type, emblem_base_type, emblem_main_color, emblem_sub_color,
                                          motto, active_days, active_time, characteristic, is_publish,
                                          comment, board_message, created)
             VALUES %s \
             """

def make_batch(n):
    # Bulk-generate bitfields for motto, days, time, characteristic
    bitfields = rng.integers(0, 2 ** 16, size=(n, 4))

    rows = []
    for i in range(n):
        # Cast numpy ints to Python ints for timedelta
        days_delta = int(rng.integers(0, 365))
        active_days = bitfields[i, 1]
        active_time = bitfields[i, 2]
        characteristic = bitfields[i, 3]

        now = datetime.now() - timedelta(days=days_delta)

        rows.append((
            int(rng.poisson(5)),  # clan_level
            int(rng.integers(1, 500)),  # member_num
            int(rng.integers(1, n)),  # master_id
            faker.boolean(), faker.boolean(), faker.boolean(),
            int(rng.exponential(3000)),  # total_clan_point
            int(rng.exponential(1500)),  # money_clan_point
            faker.company(),  # name
            faker.bothify('??##'),  # short_name
            int(rng.integers(0, 11)),  # emblem_mark_type
            int(rng.integers(0, 11)),  # emblem_base_type
            int(rng.integers(0, 51)),  # emblem_main_color
            int(rng.integers(0, 51)),  # emblem_sub_color
            int(bitfields[i, 0]),  # motto
            int(active_days),  # active_days
            int(active_time),  # active_time
            int(characteristic),  # characteristic
            faker.boolean(),  # is_publish
            faker.text(max_nb_chars=30),  # comment
            faker.sentence(),  # board_message
            now  # created (native datetime)
        ))
    return rows

t0 = time.time()
inserted = 0
while inserted < ROWS:
    count = min(BATCH_SIZE, ROWS - inserted)
    batch = make_batch(count)
    execute_values(cur, insert_sql, batch, page_size=BATCH_SIZE)
    conn.commit()
    inserted += count
    print(f"[{inserted}/{ROWS}] rows loaded", flush=True)

print(f"Total load time: {time.time() - t0:.2f}s")
cur.close()
conn.close()
