using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Game over and restart.
/// </summary>
public class GameOverRestart : MonoBehaviour
{
	#region Class Variables

	/// <summary>
	/// The game over label.
	/// </summary>
	private Text gameOverLabel;
	#endregion

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start ()
	{
		gameOverLabel = GetComponentInChildren <Text> ();

		// Event action (listener) to be executed when called
		System.Action<EventData> OnGameOver = (obj) => {

			// Activate gameobject
			gameObject.SetActive (true);


			if (((EventData) obj).Data != null) {
			
				string winner = (((PlayerType) ((EventData) obj).Data) == PlayerType.User) ? "You Won" : "You Lost";
				gameOverLabel.text = "GAMEOVER" + "\n" + winner;
			}
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnGameOver, "OnGameOver");

		gameObject.SetActive (false);
	}

	/// <summary>
	/// Restarts a new Game.
	/// </summary>
	public void OnRestartGame ()
	{
		// Deactivate gameobject
		gameObject.SetActive (false);

		// Dispatch event to notify start new game.
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "RestartGame");
	}
}
