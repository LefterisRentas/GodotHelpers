using System;
using System.Threading;
using System.Threading.Tasks;
using Godot.RPG.Scripts.Networking.Abstractions;

namespace Godot.RPG.Scripts.Networking.Implementations;

public class NetworkManager : INetworkManager
{
    private static INetworkPeer _peer = NetworkPeer.Instance;

    // ReSharper disable once InconsistentNaming
    private static readonly SemaphoreSlim _semaphore = new(1, 1); // Semaphore for thread safety

    // ReSharper disable once InconsistentNaming
    private static readonly Lazy<NetworkManager> _instance = new(() => new NetworkManager());

    // Private constructor to enforce singleton pattern
    private NetworkManager()
    {
    }

    public static NetworkManager Instance => _instance.Value;

    public async Task<bool> Connect(string address, ushort port, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.Connect(address, port, cancellationToken);
            if (!success)
            {
                Console.Error.WriteLine("Failed to connect to server.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error while trying to connect: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> Disconnect(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.Disconnect(cancellationToken);
            if (!success)
            {
                Console.Error.WriteLine("Failed to disconnect from server.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during disconnection: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> SendData(byte[] data, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.SendData(data, cancellationToken);
            if (!success)
            {
                Console.Error.WriteLine("Failed to send data to server.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error while sending data: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public INetworkPeer GetPeer()
    {
        _semaphore.Wait();
        try
        {
            return _peer;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void SetPeer(INetworkPeer peer)
    {
        _semaphore.Wait();
        try
        {
            _peer = peer ?? throw new ArgumentNullException(nameof(peer));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> StartHost(ushort port, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.StartHost(port, cancellationToken);
            return success;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error starting host: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> StopHost(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.StopHost(cancellationToken);
            return success;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error stopping host: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> StartServer(ushort port, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.StartServer(port, cancellationToken);
            return success;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error starting server: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> StopServer(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var success = await _peer.StopServer(cancellationToken);
            return success;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error stopping server: {ex.Message}");
            return false;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}