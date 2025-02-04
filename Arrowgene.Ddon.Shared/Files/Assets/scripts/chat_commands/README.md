# Chat Commands

> [!WARNING]
> Chat commands provided by the server repository and are not labeled as `AccountStateType.User` or `AccountStateType.GameMaster` are intended to be used for debug only.
> If command settings are changed such that they become available for general use, that command is to be maintained and debugged by the admins running that server.

Chat commands are behaviors which can be executed in game, starting with `/`. Commands are restricted based on the AccountType. When implementing the command, the `AccountState` field controls who can execute it. This also controls which commands show up when using the command `/help`.

Each chat command should be named as `<chat_command>.csx` and stored in the directory `<assets>/scripts/chat_commands`. The name of the file should match the name assigned to the field `CommandName`. If a command is complex and requires it's own settings file, they should be located in a file located in  `<assets>/scripts/settings/chat_commands`. The name of the settings file should match the name of the chat command.

General settings for all chat commands can be found in `<assets>/scripts/settings/ChatCommands.csx`.

## Guidelines

The value assign to `CommandName` should be lowercase and contain no spaces.