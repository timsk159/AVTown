// (c)2013 MuchDifferent. All Rights Reserved.

/// <summary>
/// The information that we are storing for each session.
/// </summary>
#if IsServer
public class SessionInfo
{
	/// <summary>
	/// The group id that we are referencing.
	/// </summary>
	public readonly int id;

	/// <summary>
	/// Number of players in the group.
	/// </summary>
	public int playerCount;

	public SessionInfo(int id, int playerCount)
	{
		this.id = id;
		this.playerCount = playerCount;
	}
}
#endif