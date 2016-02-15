using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// Time tracker.
/// </summary>
public class TimeTracker : MonoBehaviour
{
	#region Class Variables
	/// <summary>
	/// The timer label component.
	/// </summary>
	private Text timerLabel;

	/// <summary>
	/// timer holder in seconds.
	/// </summary>
	private int seconds = 0;
	#endregion

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start ()
	{
		timerLabel = GetComponent<UnityEngine.UI.Text>();

		// Event action (listener) to be executed when called
		System.Action<EventData> OnRestartGame = (obj) => {
			
			// Setting initial values
			ResetTimer();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnRestartGame, "RestartGame");

		// Event action (listener) to be executed when called
		System.Action<EventData> OnGameOver = (obj) => {

			// Stop timer
			StopTimer ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnGameOver, "OnGameOver");
	
		// Setting initial values
		ResetTimer();
	}

	/// <summary>
	/// Resets the timer.
	/// </summary>
	private void ResetTimer ()
	{
		// Initialize timer.
		seconds = 0;

		// Start Invoking every 1 second.
		InvokeRepeating ("UpdateTimer", 1f, 1f);
	}

	/// <summary>
	/// Stops the timer.
	/// </summary>
	private void StopTimer ()
	{
		// Cancel any timer invoke
		if (IsInvoking ("UpdateTimer"))
			CancelInvoke ("UpdateTimer");
	}

	/// <summary>
	/// Updates the timer.
	/// </summary>
	private void UpdateTimer ()
	{
		// update timer counter
		seconds += 1;

		// Set timer display
		timerLabel.text = GetFormatedTime (seconds);
	}

	/// <summary>
	/// Gets the formated time.
	/// </summary>
	/// <returns>The formated time.</returns>
	/// <param name="_seconds">Seconds.</param>
	private string GetFormatedTime (int _seconds)
	{
		// Create a timeSpan record and return as string so it can be aplied to the UI Text
		TimeSpan time = TimeSpan.FromSeconds(_seconds);
		return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
	}
}
