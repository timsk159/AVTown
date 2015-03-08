// (c)2013 MuchDifferent. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all game sessions running on the server.
/// Only one instance of this class should be created and used.
/// This class could use an ID allocator class and an ID type instead of ints but because it would only make
/// it complex and the current implementation is enough for the sample we leave it in this stage.
/// </summary>
#if IsServer
public class GameSessionManager
{
	/// <summary>
	/// This delegate is used to define callbacks that relate to one game session.
	/// </summary>
	/// <param name="sessionId">ID number of the game session</param>
	public delegate void SessionCallback(int sessionId);

	/// <summary>
	/// This callback will be invoked whenever a new session is created.
	/// </summary>
	public SessionCallback onSessionCreated;

	/// <summary>
	/// This callback will be invoked whenever a session is destroyed.
	/// </summary>
	public SessionCallback onSessionDestroyed;

	/// <summary>
	/// Number of players per game session
	/// </summary>
	public int maxPlayersPerSession { get; private set; }

	/// <summary>
	/// Maximum number of sessions per game server
	/// </summary>
	public int maxSessions { get; private set; }

	/// <summary>
	/// Allocated group numbers
	/// </summary>
	private Dictionary<int, SessionInfo> _runningGameSessions = new Dictionary<int, SessionInfo>();

	/// <summary>
	/// A mapping between NetworkPlayers and the group that they are playing in as their game session.
	/// </summary>
	private Dictionary<uLink.NetworkPlayer, SessionInfo> _playerToSessionMapping =
		new Dictionary<uLink.NetworkPlayer, SessionInfo>();

	/// <summary>
	/// The index of the last allocated group
	/// </summary>
	private int _lastUsedGroupIndex = 1;

	/// <summary>
	/// Returns number of current game sessions.
	/// </summary>
	public int sessionCount
	{
		get
		{
			return _runningGameSessions.Count;
		}
	}

	/// <summary>
	/// Initializes the session manager.
	/// </summary>
	/// <param name="maxPlayersPerSession">Maximum number of players per game session.</param>
	/// <param name="maxSessions">Maximum number of sessions.</param>
	public GameSessionManager(int maxPlayersPerSession, int maxSessions)
	{
		if (maxPlayersPerSession <= 0) throw new ArgumentException("Maximum number players per session must be greater than zero. (You set it to " + maxPlayersPerSession + ").", "maxPlayersPerSession");
		if (maxSessions <= 0) throw new ArgumentException("Maximum number of sessions must be greater than zero. (You set it to " + maxSessions + ").", "maxSessions");
		if (maxSessions > 65536) throw new ArgumentException("The maximum allowed sessions per server is 65536. (You set it to " + maxSessions + ").", "maxSessions");
		
		this.maxSessions = maxSessions;
		this.maxPlayersPerSession = maxPlayersPerSession;
	}

	/// <summary>
	/// Gets the number of players in a game session.
	/// </summary>
	/// <param name="sessionID">The session ID.</param>
	/// <returns>The number of players in the session.</returns>
	public int GetPlayerCountInSession(int sessionID)
	{
		SessionInfo sessionInfo;
		if (_runningGameSessions.TryGetValue(sessionID, out sessionInfo))
		{
			return sessionInfo.playerCount;
		}
		throw new Exception("Couldn't find any session with ID " + sessionID + ".");
	}

	/// <summary>
	/// Gets the ID of the session that a given player is playing in.
	/// </summary>
	/// <param name="player">The player that you want to get information for.</param>
	/// <returns>The ID of the session the player is a member of.</returns>
	public int GetSessionOfPlayer(uLink.NetworkPlayer player)
	{
		SessionInfo sessionInfo;
		if (_playerToSessionMapping.TryGetValue(player, out sessionInfo))
		{
			return sessionInfo.id;
		}
		throw new Exception("Couldn't find player " + player + " in any session.");
	}

	/// <summary>
	/// Adds the player to the first available session is one can be found.
	/// </summary>
	/// <param name="player">The player to add.</param>
	/// <param name="sessionId">The resulting session ID that the player was added to.</param>
	/// <returns>True if the player could be added, otherwise false.</returns>
	public bool TryAddPlayerToAnySession(uLink.NetworkPlayer player, out int sessionId)
	{
		// Try to find a usable group.
		SessionInfo session;
		if (TryFindSessionForNewPlayer(out session))
		{
			AddPlayerToSession(player, session);
			sessionId = session.id;
			return true;
		}

		sessionId = default(int);
		return false;
	}

	/// <summary>
	/// Adds the player to the specified session if possible.
	/// </summary>
	/// <param name="player">The player to add.</param>
	/// <param name="sessionId">Session ID to add the player to.</param>
	/// <returns>True if the player could be added, otherwise false.</returns>
	public bool TryAddPlayerToSession(uLink.NetworkPlayer player, int sessionId)
	{
		SessionInfo session;
		if (_runningGameSessions.TryGetValue(sessionId, out session) &&
			session.playerCount < maxPlayersPerSession)
		{
   			AddPlayerToSession(player, session);
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Adds a player to the session that another player is playing in, if possible.
	/// </summary>
	/// <param name="player">The player to add.</param>
	/// <param name="otherPlayer">The other player whose session the new player should join.</param>
	/// <returns>True if the player could be added, otherwise false.</returns>
	public bool TryAddPlayerToOtherPlayerSession(uLink.NetworkPlayer player, uLink.NetworkPlayer otherPlayer)
	{
		// Let's see if the other player is in any session or not.
		SessionInfo session;
		if (!_playerToSessionMapping.TryGetValue(otherPlayer, out session))
		{
			// The other player was not in any session.
			return false;
		}
		
		// Found the other player's session. Let's try to join it.
		return TryAddPlayerToSession(player, session.id);
	}

	/// <summary>
	/// Removes a player from its session.
	/// </summary>
	/// <param name="player">The player to remove.</param>
	/// <returns>True if the player was found and removed, or false if the player was not found.</returns>
	public bool RemovePlayer(uLink.NetworkPlayer player)
	{
		SessionInfo session;
		if (_playerToSessionMapping.TryGetValue(player, out session))
		{
			session.playerCount--;
			if (session.playerCount == 0)
			{
				DeallocateSession(session);
			}
			_playerToSessionMapping.Remove(player);

			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Adds the player to the given session, without any error checking.
	/// </summary>
	/// <param name="player">The player to add.</param>
	/// <param name="session">The session that the player should join.</param>
	private void AddPlayerToSession(uLink.NetworkPlayer player, SessionInfo session)
	{
		session.playerCount++;
		_playerToSessionMapping.Add(player, session);
		
		uLink.Network.AddPlayerToGroup(player, session.id);
		uLink.Network.SetGroupFlags(session.id, uLink.NetworkGroupFlags.HideGameObjects);

		if (session.playerCount == 1 && onSessionCreated != null)
		{
			onSessionCreated(session.id);
		}
	}
	
	/// <summary>
	/// Finds the next free session, if one exists.
	/// </summary>
	/// <param name="session">The resulting session.</param>
	/// <returns>True if a session with space for a new player can be found, otherwise false.</returns>
	private bool TryFindSessionForNewPlayer(out SessionInfo session)
	{
		//First let's see if we have empty positions in current sessions.
		foreach (KeyValuePair<int, SessionInfo> sessionKV in _runningGameSessions)
		{
			if (sessionKV.Value.playerCount < maxPlayersPerSession)
			{
				session = sessionKV.Value;
				return true;
			}
		}

		//If not let's allocate a new session if we can.
		return TryAllocateSession(out session);
	}

	/// <summary>
	/// Allocates a new session, if possible.
	/// </summary>
	/// <param name="session">The resulting session.</param>
	/// <returns>True if a new session could be allocated, otherwise false.</returns>
	private bool TryAllocateSession(out SessionInfo session)
	{
		//Uses the next fit algorithm and starts from the last choosen group number
		var initialIndex = _lastUsedGroupIndex;
		while (_runningGameSessions.ContainsKey(_lastUsedGroupIndex))
		{
			//simply chooses the next number and just checks for the number to don't go out of bounds in GetNext
			_lastUsedGroupIndex = GetNextIndex();
			//If the next available number is the first one that we had, then we have no more room for new sessions
			if (_lastUsedGroupIndex == initialIndex)
			{
				session = default(SessionInfo);
				return false;
			}
		}
		//Add the empty spot to the list and allocate it
		session = new SessionInfo(_lastUsedGroupIndex, 0);
		_runningGameSessions.Add(session.id, session);
		return true;
	}

	/// <summary>
	/// Deallocates a session.
	/// </summary>
	/// <param name="session">The session to deallocate.</param>
	/// <returns>True if the session was found and deallocated, otherwise false.</returns>
	private bool DeallocateSession(SessionInfo session)
	{
		if (_runningGameSessions.ContainsKey(session.id) && session.playerCount == 0)
		{
			if (onSessionDestroyed != null)
			{
				onSessionDestroyed(session.id);
			}
			
			uLink.Network.RemoveInstantiatesInGroup(session.id);
			uLink.Network.RemoveRPCsInGroup(session.id);
			
			foreach (uLink.NetworkView view in UnityEngine.Object.FindObjectsOfType(typeof(uLink.NetworkView)))
			{
				if (view.group == session.id)
				{
					uLink.Network.Destroy(view.gameObject);
				}
			}
			return true;
		}
		return false;
	}

	/// <summary>
	/// Returns the next session index which should be tried to see if it's free or not.
	/// </summary>
	/// <returns>The next session index.</returns>
	private int GetNextIndex()
	{
		//Simply checks the number to be in bound of [1,maxNumberOfSessions]. 0 means no group effectively
		return (_lastUsedGroupIndex == maxSessions) ? 1 : _lastUsedGroupIndex + 1;
	}
}
#endif