using UnityEngine;
using System.Collections;

public class PausePlayButton : MonoBehaviour
{
	private bool IsGameInPause
	{
		get {
			return Time.timeScale == 0 ? true : false;
		}
	}
	
	public void ChangePausePlay ()
	{
		if (IsGameInPause)
			SetGameTimeScale (1.0f);
		else
			SetGameTimeScale (0.0f);
	}

	private void SetGameTimeScale (float _time)
	{
		Time.timeScale = _time;
	}
}
