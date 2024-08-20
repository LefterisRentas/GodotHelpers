using System;
using System.Threading;
using System.Threading.Tasks;
using Godot.RPG.Scripts.Networking.Abstractions;

namespace Godot.RPG.Scripts.Networking.Implementations;

/// <summary>
/// <inheritdoc cref="INetworkPeer"/>
/// </summary>
public class NetworkPeer : INetworkPeer
{
	private static readonly Lazy<NetworkPeer> _instance = new(() => new NetworkPeer());
	private ENetMultiplayerPeer _enetPeer;
	private readonly MultiplayerApi _multiplayerApi;

	// Private constructor to prevent direct instantiation
	private NetworkPeer()
	{
		_enetPeer = new ENetMultiplayerPeer();
		_multiplayerApi = MultiplayerApi.CreateDefaultInterface();
		_multiplayerApi.MultiplayerPeer = _enetPeer;
	}

	// Public static property to access the singleton instance
	public static NetworkPeer Instance => _instance.Value;

	public async Task<bool> Connect(string address, ushort port, CancellationToken cancellationToken = default)
	{
		try
		{
			if (_enetPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected)
			{
				Console.Error.WriteLine("Already connected to a server.");
				return true;
			}

			var error = _enetPeer.CreateClient(address, port);
			if (error != Error.Ok)
			{
				Console.Error.WriteLine($"Failed to create client: {error}");
				return false;
			}

			// Wait for connection asynchronously
			while (_enetPeer.GetConnectionStatus() != MultiplayerPeer.ConnectionStatus.Connected)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					Console.Error.WriteLine("Connection canceled by user.");
					await Disconnect(cancellationToken);
					return false;
				}
				await Task.Delay(100, cancellationToken); // Prevent busy-waiting
			}

			Console.WriteLine("Successfully connected to server.");
			return true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error during connection: {ex.Message}");
			return false;
		}
	}

	public async Task<bool> Disconnect(CancellationToken cancellationToken = default)
	{
		try
		{
			if (_enetPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
			{
				Console.Error.WriteLine("Already disconnected.");
				return true;
			}

			_enetPeer.Close();

			// Wait for disconnection asynchronously
			while (_enetPeer.GetConnectionStatus() != MultiplayerPeer.ConnectionStatus.Disconnected)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					Console.Error.WriteLine("Disconnection canceled by user.");
					return false;
				}

				await Task.Delay(100, cancellationToken); // Prevent busy-waiting
			}

			Console.WriteLine("Successfully disconnected from server.");
			return true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error during disconnection: {ex.Message}");
			return false;
		}
	}

	public async Task<bool> SendData(byte[] data, CancellationToken cancellationToken = default)
	{
		try
		{
			if (_enetPeer.GetConnectionStatus() != MultiplayerPeer.ConnectionStatus.Connected)
			{
				Console.Error.WriteLine("Cannot send data, not connected to a server.");
				return false;
			}

			_enetPeer.PutPacket(data);
			await Task.CompletedTask; // Simulate async operation if needed

			Console.WriteLine("Data sent successfully.");
			return true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error sending data: {ex.Message}");
			return false;
		}
	}

	public async Task<bool> StartHost(ushort port, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await StartServer(port, cancellationToken);
			if (!result)
			{
				Console.Error.WriteLine("Failed to start host.");
				return false;
			}
			result = await Connect("localhost", port, cancellationToken);
			if (!result)
			{
				Console.Error.WriteLine("Failed to connect to host.");
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error during host start: {ex.Message}");
			return false;
		}
	}

	public async Task<bool> StopHost(CancellationToken cancellationToken = default)
	{
		return await Disconnect(cancellationToken); // Same as disconnecting
	}

	public Task<bool> StartServer(ushort port, CancellationToken cancellationToken = default)
	{
		try
		{
			if (_enetPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected)
			{
				Console.Error.WriteLine("Already hosting or connected to a server.");
				return Task.FromResult(false);
			}

			var error = _enetPeer.CreateServer(port);
			if (error != Error.Ok)
			{
				Console.Error.WriteLine($"Failed to start server: {error}");
				return Task.FromResult(false);
			}

			Console.WriteLine("Server started successfully.");
			return Task.FromResult(true);
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"Error during server start: {ex.Message}");
			return Task.FromResult(false);
		}
	}

	public async Task<bool> StopServer(CancellationToken cancellationToken = default)
	{
		return await Disconnect(cancellationToken); // Same as disconnecting
	}
}
