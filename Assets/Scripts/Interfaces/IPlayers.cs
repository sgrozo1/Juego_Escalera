using UnityEngine;
using System.Collections;

/// <summary>
/// I players.
/// </summary>
public interface IPlayers
{
	/// <summary>
	/// Gets a value indicating whether this instance is player turn.
	/// </summary>
	/// <value><c>true</c> if this instance is player turn; otherwise, <c>false</c>.</value>
	bool IsPlayerTurn {
		get;
	}

	/// <summary>
	/// Walks to new position.
	/// </summary>
	void WalkToNewPosition ();
}