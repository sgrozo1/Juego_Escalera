using UnityEngine;
using System.Collections;

/// <summary>
/// Quit game.
/// </summary>
public class QuitGame : MonoBehaviour
{
	/// <summary>
	/// Quits the current game.
	/// </summary>
	public void QuitCurrentGame ()
	{
		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "LoadScene", "Menu");
	}
}
