# Custom Module

The custom module is a place holder for server custom scripts. In the server repository, only the directory and this README will be tracked. The layout of this directory will match the scripts root directory. When the server first loads and it iterates over each file, it will first check to see if a copy of this file exists in `<script_root>/custom/<module>/..`. If it does, this file will be selected instead of the file provided by the server in `<script_root>/<module>/...`. If the module supports hotload, the server uses the path in the `custom` directory instead of the one located in the server provided directory. If the script only requires small edits, consider looking at using the addendum module instead, that way you can still get general script file from the server developers.

> [!WARNING]
> Now that the custom file has been added by the server admin, it is up to the server admin to make sure those files stay up to date with any changes to the original server files.

## Example: Replacing a quest

Suppose, you wanted to update the rewards of the EXM quest `q50300004`.

1. Create a new directory for the module in the `custom` directory if it doesn't exist.
  - `mkdir <script_root>/custom/quests/exm`
2. Copy the file `q50300004.csx` into `<script_root>/custom/quests/exm`
  - `cp <script_root>/quests/exm/q50300004.csx <script_root>/custom/quests/exm`
3. Make the custom edits you want to the quest file
4. Restart the server

```plaintext
2025-02-24 15:25:38 - Info - ScriptManager`1: Compiling scripts for module 'quests'
2025-02-24 15:25:38 - Info - ScriptManager`1: C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\custom\quests\exm\q50300004.csx
2025-02-24 15:25:39 - Info - ScriptManager`1: C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\quests\exm\q50300005.csx
```

Now when the server first loads, you will see that instead of selecting to load the file `<script_root>/quests/exm/q50300004.csx`, it will instead load the file `<script_root>/custom/quests/exm/q50300004.csx`.