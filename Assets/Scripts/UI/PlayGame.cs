using UnityEngine;
using System.Collections;

/// <summary>
/// Play game.
/// </summary>
public class PlayGame : MonoBehaviour
{
	/// <summary>
	/// Starts the game.
	/// </summary>
	public void StartGame ()
	{
		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "LoadScene", "Game");
	}
}
