using UnityEngine;
using System.Collections;

/// <summary>
/// Dice.
/// </summary>
public class Dice : DiceBase
{
	/// <summary>
	/// Initialize object instance values
	/// </summary>
	protected override void Start ()
	{
		base.Start ();

		// Event action (listener) to be executed when called
		System.Action<EventData> OnRollTheDice = (obj) => {

			// Reset Dice to null
			tossResult = -1;

			// Start a new roll of dice
			RollDice ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnRollTheDice, "OnRollTheDice");
	}

	/// <summary>
	/// Checks the dice toss result and Dispaches event either to move current player or to toss again.
	/// </summary>
	protected override void CheckDiceTossResult ()
	{
		// Get The Dice Result
		base.CheckDiceTossResult ();

		// Move curent player if result landed on face
		if (tossResult > 0) {

			// Dispatch event to notify that the dice has been tossed and there is a result.
			if (EventManager.Instance != null)
				EventManager.Instance.CallEvent (this, "DiceRolled", tossResult);
		} else {

			// Dispatch event to notify start new dice toss.
			if (EventManager.Instance != null)
				EventManager.Instance.CallEvent (this, "OnRollTheDice");
		}
	}
}
