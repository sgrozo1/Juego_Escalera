using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour
{
	#region Singleton Instance Variables
	/// <summary>
	/// The instance.
	/// </summary>
	private static LevelLoader instance = null;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static LevelLoader Instance
	{
		get
		{
			if (instance == null)
				instance = new LevelLoader();
			
			return instance;
		}
	}
	#endregion

	/// <summary>
	/// Awake this instance. For main initialation.
	/// </summary>
	private void Awake()
	{
		// Check if instance is set
		if(instance)
		{
			// destroy duplicate
			DestroyImmediate (gameObject);
			return;
		}

		// Mark instance and flag not to be destroyed.
		instance = this;
		DontDestroyOnLoad (gameObject);
	}
	
	// ------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Start this instance. Initializes data
	/// </summary>
	private void Start ()
	{
		// Event action (listener) to be executed when called
		System.Action<EventData> LoadNotificationLevel = (obj) => {

			// Get what is to be the next level
			if (((EventData) obj).Data != null)
				LoadLevel ((string) ((EventData) obj).Data);
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, LoadNotificationLevel, "LoadScene");	
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <param name="level">Level.</param>
	private void LoadLevel (string _level)
	{
		// Loads next scene
		SceneManager.LoadScene (_level);
	}

	/// <summary>
	/// Raises the level was loaded event.
	/// </summary>
	private void OnLevelWasLoaded ()
	{
		// Cleans unused memory (heap). This might be an explensive process depending on the amount of memory allocated.
		CallGarbageCollector ();
	}

	/// <summary>
	/// Calls the garbage collector.
	/// </summary>
	private void CallGarbageCollector ()
	{
		Resources.UnloadUnusedAssets ();
		System.GC.Collect();
	}
}
