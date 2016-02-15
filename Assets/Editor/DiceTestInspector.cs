using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(DiceTest))]
public class DiceTestInspector : Editor
{
	private DiceTest dice;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (dice == null)
			dice = (DiceTest) target;

		if (GUILayout.Button("Test roll the dice"))
			dice.TestDiceResult ();
		
	}

}
