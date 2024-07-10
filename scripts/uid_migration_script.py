#!/bin/env python
# Copyright 2024
#
# Permission is hereby granted, free of charge, to any person obtaining a copy of this
# software and associated documentation files (the “Software”), to deal in the Software
# without restriction, including without limitation the rights to use, copy, modify,
# merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
# permit persons to whom the Software is furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
# OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
# FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

import argparse
import sys
import sqlite3
import shutil

from pathlib import Path

    # item_uid, character_id, storage_type, slot_no, item_num, item_id, unk3, color, plus_value
create_storage_table_statement = \
"""
CREATE TABLE IF NOT EXISTS ddon_storage_item (
    item_uid     VARCHAR(8) NOT NULL,
    character_id INTEGER NOT NULL,
    storage_type SMALLINT NOT NULL,
    slot_no      SMALLINT NOT NULL,
    item_num     INTEGER NOT NULL,
    item_id      INTEGER NOT NULL,
    unk3         INTEGER NOT NULL,
    color        INTEGER NOT NULL,
    plus_value   INTEGER NOT NULL,
    CONSTRAINT pk_ddon_storage_item PRIMARY KEY (character_id, storage_type, slot_no),
    CONSTRAINT fk_storage_item_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);
"""

def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument("input", help="The sqlite3 database file to read")
    parser.add_argument("output", help="name of the output file to write create")

    args = parser.parse_args()

    db_file = Path(args.input)
    if not db_file.is_file():
        print(f"The path '{args.input}' is not valid. Exiting.")
        return None

    return args


def main():
    args = parse_args()
    if args is None:
        return

    con = sqlite3.connect(args.input)
    cur = con.cursor()

    # UID, ItemId, unk3, color, plus_value
    # ('9886A725', 35, 0, 0, 0)
    ddon_characters = [a for a in cur.execute("SELECT * FROM ddon_character")]
    ddon_pawns = [a for a in cur.execute("SELECT * FROM ddon_pawn")]
    ddon_items = [a for a in cur.execute("SELECT * FROM ddon_item")]
    ddon_storage_items = [a for a in cur.execute("SELECT * FROM ddon_storage_item")]
    ddon_equip_items = [a for a in cur.execute("SELECT * FROM ddon_equip_item")]
    con.close()

    character_common_mapping = {}
    for character in ddon_characters:
        # character_id, character_common_id
        character_id = character[0]
        character_common_id = character[1]
        character_common_mapping[character_common_id] = {
                "id": character_id,
                "type": "character"
        }

    for pawn in ddon_pawns:
        # pawn_id, character_common_id, character_id
        character_common_mapping[pawn[1]] = {
            "id": pawn[2],
            "type": "pawn"
        }

    item_mapping = {}
    uid = 1
    for item in ddon_items:
        old_uid = item[0]
        new_uid = f'{uid:08X}'

        if old_uid in item_mapping:
            print(f"There are duplicate UID values of {uid}")
            return

        item_mapping[old_uid] = {
            'old_uid': item[0],
            'new_uid': new_uid,
            'item_id': item[1],
            'unk3': item[2],
            'color': item[3],
            'plus_value': item[4]
        }
        uid += 1

    # item_uid, character_id, storage_type, slot_no, item_num, item_id, unk3, color, plus_value
    storage_items = []
    for storage_item in ddon_storage_items:
        old_uid = storage_item[0]
        storage_items.append(
            (item_mapping[old_uid]['new_uid'], storage_item[1], storage_item[2], storage_item[3], storage_item[4],
            item_mapping[old_uid]['item_id'], item_mapping[old_uid]['unk3'], item_mapping[old_uid]['color'], item_mapping[old_uid]['plus_value'])
        )

    # item_uid, character_common_id, job, equip_type, equip_slot
    for equip_item in ddon_equip_items:
        old_uid = equip_item[0]
        new_uid = uid

        if equip_item[1] not in character_common_mapping:
            continue

        character_id = character_common_mapping[equip_item[1]]["id"]
        storage_items.append(
            (new_uid, character_id, 6, 0, 1, item_mapping[old_uid]['item_id'], item_mapping[old_uid]['unk3'], item_mapping[old_uid]['color'], item_mapping[old_uid]['plus_value'])
        )

        uid += 1

    shutil.copyfile(args.input, args.output)
    con = sqlite3.connect(args.output)
    cur = con.cursor()

    cur.execute("DROP TABLE ddon_item")
    cur.execute("DROP TABLE ddon_storage_item")
    cur.execute("DELETE FROM ddon_equip_item")
    cur.execute("DELETE FROM ddon_equip_job_item")
    con.commit();

    cur.execute(create_storage_table_statement)
    con.commit();

    cur.executemany("INSERT INTO ddon_storage_item VALUES (?,?,?,?,?,?,?,?,?)", storage_items)
    con.commit()
    con.close()


if __name__ == '__main__':
    main()

