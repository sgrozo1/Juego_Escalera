using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
	#region Singleton Instance Variables
	private static GameManager instance = null;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
				instance = new GameManager();

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
	/// The current player.
	/// </summary>
	[SerializeField]
	private PlayerType currentPlayer;

	/// <summary>
	/// The special slots static list.
	/// </summary>
	[SerializeField]
	private SpecialBoardSlots[] specialSlots = new SpecialBoardSlots[14];

	/// <summary>
	/// Gets the get current player.
	/// </summary>
	/// <value>The get current player.</value>
	public PlayerType GetCurrentPlayer
	{
		get{
			return currentPlayer;
		}
	}

	/// <summary>
	/// Gets the special slot.
	/// </summary>
	/// <returns>The special slot.</returns> Return null if didnot find
	/// <param name="_slotNuber">Slot nuber.</param>
	public SpecialBoardSlots GetSpecialSlot (int _slotNuber)
	{
		return specialSlots.FirstOrDefault (x => x.SlotNumber == _slotNuber);
	}

	/// <summary>
	/// Initialize object instance values
	/// </summary>
	private void Start ()
	{
		// Event action (listener) to be executed when called
		System.Action<EventData> OnTurnFinished = (obj) => {

			// On Turn has finish update current user
			ChangeCurrentUser ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnTurnFinished, "OnTurnFinished");

		// Event action (listener) to be executed when called
		System.Action<EventData> OnRestartGame = (obj) => {

			// Setting initial values
			SetInitialCurrentUser();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnRestartGame, "RestartGame");

		// Setting initial values
		SetInitialCurrentUser ();
	}

	/// <summary>
	/// Sets the initial current user.
	/// </summary>
	private void SetInitialCurrentUser ()
	{
		// Sets hte initial user
		currentPlayer = PlayerType.User;
	}
		
	/// <summary>
	/// Changes the current user.
	/// </summary>
	private void ChangeCurrentUser ()
	{
		// Change current player
		if (GetCurrentPlayer == PlayerType.AI)
			currentPlayer = PlayerType.User;
		else
			currentPlayer = PlayerType.AI;

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "StartNewTurn");
	}
}

/// <summary>
/// Special board slots.
/// </summary>
[Serializable]
public class SpecialBoardSlots
{
	/// <summary>
	/// The slot number.
	/// </summary>
	public int SlotNumber;

	/// <summary>
	/// The target number.
	/// </summary>
	public int TargetNumber;

	/// <summary>
	/// The is ladder.
	/// </summary>
	public bool IsLadder;
}