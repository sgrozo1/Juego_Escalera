using UnityEngine;
using System.Collections;

/// <summary>
/// Player base.
/// </summary>
public class PlayerBase : MonoBehaviour, IPlayers
{
	#region Class Variables
	/// <summary>
	/// The BOARD MAX PLACE (position).
	/// </summary>
	private const float MOVING_SPEED = 4.0f;

	/// <summary>
	/// The BOARD MAX PLACE (position).
	/// </summary>
	private const int BOARD_MAX_PLACE = 100;

	[SerializeField]
	/// <summary>
	/// Players current game position. (position on board)
	/// </summary>
	private int currentGamePosition = 1;

	[SerializeField]
	/// <summary>
	/// Player Type
	/// </summary>
	private PlayerType playerType;

	/// <summary>
	/// The active path. (Max size three because can only move from one row to the next)
	/// </summary>
	private Vector3[] activeTargetPath = new Vector3 [3];

	/// <summary>
	/// The renderer component.
	/// </summary>
	private Renderer rendererComponent;

	[SerializeField]
	/// <summary>
	/// The color of the object.
	/// </summary>
	private Color objectColor;
	#endregion

	/// <summary>
	/// Gets a value indicating whether is player turn.
	/// </summary>
	/// <value><c>true</c> if Game Manager current player equals player turn; otherwise, <c>false</c>.</value>
	public bool IsPlayerTurn
	{
		get{
			return playerType == GameManager.Instance.GetCurrentPlayer ? true : false;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="PlayerBase"/> has finish the game.
	/// </summary>
	/// <value><c>true</c> if check game over; otherwise, <c>false</c>.</value>
	private bool CheckGameOver
	{
		get {
			return currentGamePosition == BOARD_MAX_PLACE ? true : false;
		}
	}

	private bool IsNextPathAvailable (int _currentIndex)
	{
		if (_currentIndex == 3)
			return false;

		if (activeTargetPath [_currentIndex] == Vector3.zero)
			return false;
		
		return true;
	}

	/// <summary>
	/// Determines whether player can walk those steps the specified _diceResult (if its not going to over pass the end of the game).
	/// </summary>
	/// <returns><c>true</c> if _diceResult plus current position is less than game board max place; otherwise, <c>false</c>.</returns>
	/// <param name="_diceResult">Dice result.</param>
	private bool CanWalkThoseSteps (int _diceResult)
	{
		return currentGamePosition + _diceResult > BOARD_MAX_PLACE ? false: true;
	}

	/// <summary>
	/// Initialize object instance values
	/// </summary>
	private void Start ()
	{
		rendererComponent = GetComponent <Renderer> ();

		// Event action (listener) to be executed when called
		System.Action<EventData> OnDiceRolled = (obj) => {

			// Checks (filters) wheather is the players turn
			if (!IsPlayerTurn)
				return;
				
			// Call Method for all users. Any filtering will be done by each candidate (player) 
			OnDiceHasToss ((int) ((EventData) obj).Data);
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnDiceRolled, "DiceRolled");

		// Event action (listener) to be executed when called
		System.Action<EventData> OnStartNewTurn = (obj) => {

			// Enable or Disable active players turn
			objectColor.a = IsPlayerTurn ? 1.0f : 0.5f;

			// Update the color
			rendererComponent.material.color = objectColor;

			// Checks (filters) wheather is the players turn
			if (!IsPlayerTurn)
				return;

			// AI toss dice event
			if (GameManager.Instance.GetCurrentPlayer == PlayerType.AI)
				AutoTossDice ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnStartNewTurn, "StartNewTurn");

		// Event action (listener) to be executed when called
		System.Action<EventData> OnRestartGame = (obj) => {

			// Setting start position and settings
			SetInitialSettings ();
		};

		// Suscribing listener to event manager
		if (EventManager.Instance != null)
			EventManager.Instance.AddListener (this, OnRestartGame, "RestartGame");


		// Setting start position and settings
		SetInitialSettings ();
	}

	/// <summary>
	/// Automatic toss dice event.
	/// </summary>
	private void AutoTossDice ()
	{
		// Dispatch event to notify start new dice toss.
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "OnRollTheDice");
	}

	/// <summary>
	/// Sets the initial settings.
	/// </summary>
	private void SetInitialSettings ()
	{
		// Set to inital position.
		currentGamePosition = 1;
		transform.localPosition = Helpers.GetCellPosition (currentGamePosition);

		// Enable or Disable active players turn
		objectColor.a = IsPlayerTurn ? 1.0f : 0.5f;
	}

	/// <summary>
	/// Process The dice toss Results.
	/// </summary>
	/// <param name="_diceResult">Dice result.</param>
	private void OnDiceHasToss (int _diceResult)
	{
		if (IsInvoking ("FinishMove"))
			CancelInvoke ("FinishMove");
		
		// Checks wheather it preform that movement or not.
		if (CanWalkThoseSteps (_diceResult)) {

			// Set walking path and execute.
			ExecuteWalk ((currentGamePosition + _diceResult));
		} else {

			// Could not move so it changes turn 
			Invoke ("FinishMove", 0.5f);
		}
	}

	/// <summary>
	/// Executes the walk.
	/// </summary>
	/// <param name="_diceResult">Dice result.</param>
	private void ExecuteWalk (int _targetSlotNumber, bool _followBoardPath = true)
	{
		// Set the walking path.
		SetWalkingPath (_targetSlotNumber, _followBoardPath);

		// Execute the walk to the new position.
		WalkToNewPosition ();
	}

	/// <summary>
	/// Sets the walking path.
	/// </summary>
	/// <param name="_diceResult">Dice result.</param>
	private void SetWalkingPath (int _targetSlotNumber, bool _followBoardPath)
	{
		// Cleans active path (this ensures us that it does not move something different)
		activeTargetPath = new Vector3[3];

		//Defines walking path
		Vector2 currentPosition = Helpers.GetCellIndex (currentGamePosition);
		Vector2 targetPosition = Helpers.GetCellIndex (_targetSlotNumber);

		if (_followBoardPath) {

			// Check for row change
			if (currentPosition.y != targetPosition.y) {

				// Sets path until end of row.
				int lastPlaceInRow = Helpers.LastCellInRow ((int)currentPosition.y);
				activeTargetPath [0] = Helpers.GetCellPosition (lastPlaceInRow);

				// Check if Start of new row is End of path.
				if ((lastPlaceInRow + 1) == _targetSlotNumber) {

					// Set end of Path.
					activeTargetPath [1] = Helpers.GetCellPosition (_targetSlotNumber);
				} else {

					// Set change in row
					activeTargetPath [1] = Helpers.GetCellPosition ((lastPlaceInRow + 1));

					// Set end of Path.
					activeTargetPath [2] = Helpers.GetCellPosition (_targetSlotNumber);
				}
			} else {

				// Set end of Path.
				activeTargetPath [0] = Helpers.GetCellPosition (_targetSlotNumber);
			}
		} else {
			
			// Set end of Path.
			activeTargetPath [0] = Helpers.GetCellPosition (_targetSlotNumber);

			// TODO: It would be better to follow the sanke with a transform animation
		}

		// Set new current (target) position.
		currentGamePosition = _targetSlotNumber;
	}

	/// <summary>
	/// Walks to new position on game board.
	/// </summary>
	public void WalkToNewPosition ()
	{
		// Walk to target
		StartCoroutine (DoWalk ());
	}

	/// <summary>
	/// Walks to new position on game board.
	/// </summary>
	private IEnumerator DoWalk ()
	{
		// Path Index Counter
		int pathIndex = 0;

		// Loop throw walking path.
		while (IsNextPathAvailable (pathIndex)) {

			// Set Start and end positions
			Vector3 startPosition = Vector3.zero;
			Vector3 targetPosition = Vector3.zero;

			// Define a rate and itineration counters for a smooth move
			float i;
			float rate;

			// Reset new temporary start position for next path walk
			startPosition = transform.localPosition;

			// Set new target position if target is ont Vector.zero
			targetPosition = activeTargetPath [pathIndex];

			i = 0.0f;
			rate = MOVING_SPEED/Vector3.Distance (startPosition, targetPosition);

			// Loop throw distance to target (smooth walk).
			while (i < 1.0f) {

				// Adjust rate for smooth walk
				i += Time.deltaTime * rate;

				// Move player to target position
				transform.localPosition = Vector3.Lerp (startPosition, targetPosition, i);
				yield return 0; 
			}

			// Set new itineration position
			pathIndex++;

			yield return 0; 
		}

		// Check if target number is a special slot
		SpecialBoardSlots specialSlot = GameManager.Instance.GetSpecialSlot (currentGamePosition);

		if (specialSlot != null) {

			// Execute new movement due to land on special place
			ExecuteWalk (specialSlot.TargetNumber, false);
		}
		else {
			// Has game ended?
			if (CheckGameOver) {
				
				// Dispatch event to notify that the game is over.
				if (EventManager.Instance != null)
					EventManager.Instance.CallEvent (this, "OnGameOver", playerType);
			} else {
				
				// Notify it has finish moving.
				FinishMove ();
			}
		}
	}

	/// <summary>
	/// Calls and event when player (User or AI) has Finishs moving.
	/// </summary>
	private void FinishMove ()
	{
		// Dispatch event to notify end of turn.
		if (EventManager.Instance != null)
			EventManager.Instance.CallEvent (this, "OnTurnFinished");
	}
}