using System.Threading;
using System.Threading.Tasks;

namespace Godot.RPG.Scripts.Networking.Abstractions;

/// <summary>
/// Interface representing a network peer capable of connecting, disconnecting, and sending data.
/// </summary>
public interface INetworkPeer
{
    /// <summary>
    /// Connects to a server at the specified address and port.
    /// </summary>
    /// <param name="address">The IP address or hostname of the server.</param>
    /// <param name="port">The port number to connect to.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success or failure.</returns>
    Task<bool> Connect(string address, ushort port, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success or failure.</returns>
    Task<bool> Disconnect(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends data to the server.
    /// </summary>
    /// <param name="data">The byte array containing the data to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success or failure.</returns>
    Task<bool> SendData(byte[] data, CancellationToken cancellationToken = default);
    /// <summary>
    /// Starts hosting a server on the specified port and connects to it.
    /// </summary>
    /// <param name="port"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> StartHost(ushort port, CancellationToken cancellationToken = default);
    /// <summary>
    /// Stops hosting the server and disconnects all clients.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> StopHost(CancellationToken cancellationToken = default);
    /// <summary>
    /// Starts a server on the specified port.
    /// </summary>
    /// <param name="port"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> StartServer(ushort port, CancellationToken cancellationToken = default);
    /// <summary>
    /// Stops the server and disconnects all clients.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> StopServer(CancellationToken cancellationToken = default);
}