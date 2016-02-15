using UnityEngine;
using System.Collections;

public class DiceBase : MonoBehaviour
{
	#region Class Variables
	/// <summary>
	/// Distance Offset to center of screen.
	/// </summary>
	private const float DISTANCE_TO_CENTER = 6f;

	[SerializeField]
	/// <summary>
	/// The dice number.
	/// </summary>
	protected int diceNumber;

	/// <summary>
	/// The toss result.
	/// </summary>
	protected int tossResult = -1;

	/// <summary>
	/// The rbody component.
	/// </summary>
	private Rigidbody rbody;
	#endregion

	/// <summary>
	/// Initialize object instance values
	/// </summary>
	protected virtual void Start ()
	{
		// Set reference to its own rigidbody
		rbody = GetComponent<Rigidbody> ();
	}

	/// <summary>
	/// Determines whether it can toss the dice.
	/// </summary>
	/// <returns><c>true</c> if dice is still; otherwise, <c>false</c>.</returns>
	protected bool IsDiceMoving ()
	{
		if (!rbody.IsSleeping () || rbody.velocity.magnitude != 0.0f)
			return true;
		else
			return false;
	}

	/// <summary>
	/// Rolls the dice.
	/// </summary>
	protected void RollDice ()
	{
		// Roll the dice.
		SetDiceImpulse ();

		// Start a coroutine to check that dice has stopped.
		StartCoroutine (WaitForDiceToStop ());
	}

	/// <summary>
	/// Sets the dice impulse.
	/// </summary>
	private void SetDiceImpulse ()
	{
		// Check distance to center if close then throw dice in any direction else throw it towards the middle
		if (Vector3.Distance (transform.position, Vector3.zero) < DISTANCE_TO_CENTER)
			rbody.AddForce (new Vector3 (Random.Range (-10f, 10f), Random.Range (-10f, 10f), 0f), ForceMode.Impulse);	
		else
			rbody.AddForce (-transform.position * 0.5f, ForceMode.Impulse);

		//Add an upper impulse
		rbody.AddForce(Vector3.back * Random.Range(18.0f, 25.0f), ForceMode.Impulse);

		// Add angular movement
		rbody.angularVelocity += -Vector3.forward * Random.Range (50.0f, 150.0f);
		rbody.angularVelocity += Vector3.right * Random.Range (50.0f, 150.0f);
	}

	/// <summary>
	/// Wait for dice to stop.
	/// </summary>
	/// <returns>The dice stop.</returns>
	private IEnumerator WaitForDiceToStop ()
	{
		// Wait Till dice stops
		while (IsDiceMoving())
			yield return 0; 

		// Get Toss Result
		CheckDiceTossResult ();
	}

	/// <summary>
	/// Checks the dice toss result and Dispaches event either to move current player or to toss again.
	/// </summary>
	protected virtual void CheckDiceTossResult ()
	{
		// Get The Dice Result
		tossResult = GetDiceResult ();
	}

	/// <summary>
	/// Get dice result.
	/// </summary>
	/// <returns>The dice result, returns face or -1 if not landed on face</returns>
	private int GetDiceResult ()
	{
		if (transform.up == Vector3.forward)
			return 2;

		if (transform.up == Vector3.forward * -1)
			return 5;

		if (transform.forward == Vector3.forward)
			return 6;

		if (transform.forward == Vector3.forward * -1)
			return 1;

		if (transform.right == Vector3.forward)
			return 3;

		if (transform.right == Vector3.forward * -1)
			return 4;

		return 0;
	}
}