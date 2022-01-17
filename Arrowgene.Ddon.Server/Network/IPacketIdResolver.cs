namespace Arrowgene.Ddon.Shared
{
    public interface IPacketIdResolver
    {
        PacketId Get(byte groupId, ushort handlerId, byte handlerSubId);
        ServerType ServerType { get; }
    }

    public static class PacketIdResolver
    {
        public static LoginPacketIdResolver LoginPacketIdResolver = new LoginPacketIdResolver();
        public static GamePacketIdResolver GamePacketIdResolver = new GamePacketIdResolver();
    }

    public class LoginPacketIdResolver : IPacketIdResolver
    {
        public PacketId Get(byte groupId, ushort handlerId, byte handlerSubId)
        {
            return PacketId.GetLoginPacketId(groupId, handlerId, handlerSubId);
        }

        public ServerType ServerType => ServerType.Login;
    }

    public class GamePacketIdResolver : IPacketIdResolver
    {
        public PacketId Get(byte groupId, ushort handlerId, byte handlerSubId)
        {
            return PacketId.GetGamePacketId(groupId, handlerId, handlerSubId);
        }

        public ServerType ServerType => ServerType.Game;
    }
}
