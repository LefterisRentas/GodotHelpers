using System.Threading;
using System.Threading.Tasks;

namespace Godot.RPG.Scripts.Networking.Abstractions;

public interface INetworkManager
{
    Task<bool> Connect(string address, ushort port, CancellationToken cancellationToken = default);
    Task<bool> Disconnect(CancellationToken cancellationToken = default);
    Task<bool> SendData(byte[] data, CancellationToken cancellationToken = default);

    static Task<INetworkManager> GetInstance(CancellationToken cancellationToken = default)
    {
        return default;
    }

    INetworkPeer GetPeer();
    void SetPeer(INetworkPeer peer);
    Task<bool> StartHost(ushort port, CancellationToken cancellationToken = default);
    Task<bool> StopHost(CancellationToken cancellationToken = default);
    Task<bool> StartServer(ushort port, CancellationToken cancellationToken = default);
    Task<bool> StopServer(CancellationToken cancellationToken = default);
}