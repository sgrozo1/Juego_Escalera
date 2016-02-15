using UnityEngine;
using System.Collections;

/// <summary>
/// Toss dice button actions.
/// </summary>
public class TossDiceButton : MonoBehaviour
{
	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start ()
	{
		// Event action (listener) to be executed when called
		System.Action<EventData> OnStartNewTurn = (obj) => {

			// Activate the dice button
			ActivateDiceButton ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnStartNewTurn, "StartNewTurn");

		// Event action (listener) to be executed when called
		System.Action<EventData> OnRestartGame = (obj) => {

			// Activate dice button
			ActivateDiceButton();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnRestartGame, "RestartGame");
	}

	/// <summary>
	/// Activates the dice button.
	/// </summary>
	private void ActivateDiceButton ()
	{
		// Activate gameobject if its users turn
		if (GameManager.Instance.GetCurrentPlayer == PlayerType.User)
			gameObject.SetActive (true);
	}

	/// <summary>
	/// Calls the toss dice.
	/// </summary>
	public void CallTossDice ()
	{
		// Dispatch event to notify start new dice toss.
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "OnRollTheDice");

		gameObject.SetActive (false);
	}
}
