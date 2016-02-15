using UnityEngine;
using System.Collections;

/// <summary>
/// Dice test.
/// </summary>
public class DiceTest : DiceBase
{
	public void TestDiceResult ()
	{
		RollDice ();
	}


	/// <summary>
	/// Checks the dice toss result and Dispaches event either to move current player or to toss again.
	/// </summary>
	protected override void CheckDiceTossResult ()
	{
		// Get The Dice Result
		base.CheckDiceTossResult ();

		Debug.Log (tossResult);
	}
}
