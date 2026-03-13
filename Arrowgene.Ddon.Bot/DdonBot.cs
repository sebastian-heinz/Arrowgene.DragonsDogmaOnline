using System.Net;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Bot;

/// <summary>
/// A bot acts as a real entity on the server.
/// It uses network communication to interact with the game server.
/// </summary>
public class DdonBot
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonBot));

    private readonly DdonTcpClient _client;
    private readonly Queue<BotAction> _actions;
    private readonly BotActionParser _parser;
    private readonly Challenge _challange;
    private readonly PacketFactory _packetFactory;
    private readonly ServerType _serverType;

    private string _identity;
    private bool _challangeCompleted;

    public DdonBot()
    {
        _identity = $"DdonBot-{Guid.NewGuid()}";
        _serverType = ServerType.Login;
        _actions = new Queue<BotAction>();
        _parser = new BotActionParser();
        _client = new DdonTcpClient();
        _client.DataReceived += ClientOnDataReceived;
        _client.Connected += ClientOnConnected;
        _client.Disconnected += ClientOnDisconnected;
        _client.Error += ClientOnError;
        _challangeCompleted = false;
        _challange = new Challenge();
        if (_serverType == ServerType.Login)
        {
            _packetFactory = new PacketFactory(new ServerSetting(), PacketIdResolver.LoginPacketIdResolver);
        }
        else if (_serverType == ServerType.Game)
        {
            _packetFactory = new PacketFactory(new ServerSetting(), PacketIdResolver.LoginPacketIdResolver);
        }
    }

    public void Connect(string ip, int port)
    {
        _client.Start(ip, port);
    }

    public void Connect(IPEndPoint endPoint)
    {
        Connect(endPoint.Address.ToString(), endPoint.Port);
    }

    public void Disconnect()
    {
        _client.Stop();
    }

    public void Execute(BotAction action)
    {
        // todo make this thread safe
        _actions.Enqueue(action);
    }

    private void RunActions()
    {
        // start this in a thread, only after challange completed
        // dequeue action
        // execute action
        //action.Execute(this);
    }

    public void Execute(string action)
    {
        BotAction? botAction = _parser.Parse(action);
        if (botAction == null)
        {
            Logger.Error($"{_identity}: Failed to parse action: {action}");
            return;
        }

        Execute(botAction);
    }

    private void ClientOnDataReceived(object? sender, DataReceivedEventArgs e)
    {
        byte[] recv = e.Data;

        Logger.Info($"{_identity}: Received: {recv.Length} bytes");
        if (!_challangeCompleted)
        {
            byte[] challenge = _packetFactory.ReadDataWithLengthPrefix(recv);
            _challange.ClientCompleteChallenge(challenge);
            byte[] key = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
            C2SCertClientChallengeReq req = _challange.ClientCreateChallengeRequest(key);
            StructurePacket sp = new StructurePacket<C2SCertClientChallengeReq>(req);
            byte[] data = _packetFactory.WriteDataWithLengthPrefix(sp.Data);
            SendRaw(data);

            // TODO should it be set only after server has responded?
            _packetFactory.SetCamelliaKey(key);

            // this flag defensibility should be set later before allowing actions.
            _challangeCompleted = true;
        }

        List<IPacket> packets;
        packets = _packetFactory.Read(recv);
        foreach (IPacket packet in packets)
        {
            HandlePacket(packet);
        }
    }

    private void HandlePacket(IPacket packet)
    {
        Logger.Info($"{_identity}: Received packet: {packet.Id}");
        Logger.Info($"{packet}");
    }

    private void Send(IPacket packet)
    {
        byte[] data = _packetFactory.Write(packet);
        SendRaw(data);
    }

    private void SendRaw(byte[] data)
    {
        _client.SendAsync(data).GetAwaiter().GetResult();
    }

    private void ClientOnConnected(object? sender, EventArgs e)
    {
        Logger.Info($"{_identity}: Connected");
    }

    private void ClientOnDisconnected(object? sender, EventArgs e)
    {
        Logger.Info($"{_identity}: Disconnected");
    }

    private void ClientOnError(object? sender, TcpClientErrorEventArgs e)
    {
        Logger.Error($"{_identity}: Exception");
        Logger.Exception(e.Exception);
    }
}
