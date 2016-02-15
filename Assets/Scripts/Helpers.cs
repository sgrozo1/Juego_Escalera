using UnityEngine;
using System.Collections;


public static class Helpers
{
	#region Constant Values
	private const int BOARD_WIDTH = 1792;
	private const int BOARD_HEIGTH = 1350;

	private const float LOWER_ADJUSTMENT_FACTOR = 0.912f;
	private const float LEFT_ADJUSTMENT_FACTOR = 0.918f;
	private const float UPPER_ADJUSTMENT_FACTOR = 0.915f;
	private const float RIGHT_ADJUSTMENT_FACTOR = 0.925f;
	#endregion

	/// <summary>
	/// Boards the aspect ratio.
	/// </summary>
	/// <returns>The aspect ratio.</returns>
	/// <param name="_invert">If set to <c>true</c> invert.</param>
	private static float BoardAspectRatio (bool _invert = false)
	{
		if (_invert)
			return BOARD_HEIGTH / (float)BOARD_WIDTH;
		
		return BOARD_WIDTH / (float)BOARD_HEIGTH;
	}

	/// <summary>
	/// Screens the aspect ratio.
	/// </summary>
	/// <returns>The aspect ratio.</returns>
	/// <param name="_invert">If set to <c>true</c> invert.</param>
	private static float ScreenAspectRatio (bool _invert = false)
	{
		if (_invert)
			return Screen.height / (float)Screen.width;

		return Screen.width / (float)Screen.height;
	}

	/// <summary>
	/// Currents the screen orientation.
	/// </summary>
	/// <returns>The screen orientation.</returns>
	private static ScreenOrientation CurrentScreenOrientation ()
	{
		if (BoardAspectRatio () > ScreenAspectRatio ())
			return ScreenOrientation.Portrait;
		else
			return ScreenOrientation.Landscape;
	}

	/// <summary>
	/// Gets the board corners.
	/// </summary>
	/// <returns>The board corners. lower left and upper right</returns>
	private static Vector4 GetBoardCorner ()
	{
		Vector4 returnData = new Vector4 ();

		float cameraSize = Camera.main.orthographicSize;

		if (CurrentScreenOrientation () == ScreenOrientation.Landscape) {
			returnData.Set (BoardAspectRatio () * cameraSize * LEFT_ADJUSTMENT_FACTOR * -1f,
				cameraSize * LOWER_ADJUSTMENT_FACTOR * -1f,
				BoardAspectRatio () * cameraSize * RIGHT_ADJUSTMENT_FACTOR,
				cameraSize * UPPER_ADJUSTMENT_FACTOR);
		
		} else { //TODO: there is something wrong here!!
			returnData.Set (cameraSize * LEFT_ADJUSTMENT_FACTOR * -1f,
				BoardAspectRatio (true) * cameraSize * LOWER_ADJUSTMENT_FACTOR * -1f,
				cameraSize * RIGHT_ADJUSTMENT_FACTOR,
				BoardAspectRatio (true) * cameraSize * UPPER_ADJUSTMENT_FACTOR);
		}
			
		return returnData;
	}

	// TODO: We need a scaler function for non UI elements, specially when changes orientation!!

	/// <summary>
	/// Gets the position (Vector3) of a given board number.
	/// </summary>
	/// <returns>The cell position.</returns>
	/// <param name="_number">Number.</param>
	public static Vector3 GetCellPosition (int _number)
	{
		Vector2 cellPosition = GetCellIndex (_number);
		Vector4 boardGame =  Helpers.GetBoardCorner ();

		float xDist = (boardGame.z - boardGame.x) / 10f;
		float yDist = (boardGame.w - boardGame.y) / 10f;

		return new Vector3 ((boardGame.x + (xDist * cellPosition.x)) - (xDist/2f),
							(boardGame.y + (yDist * cellPosition.y)) - (yDist/2f),
							-0.05f);
	}

	/// <summary>
	/// Lasts the cell in row.
	/// </summary>
	/// <returns>The cell number of the last slot in row.</returns>
	/// <param name="_row">Row.</param>
	public static int LastCellInRow (int _row)
	{
		return _row * 10;
	}

	/// <summary>
	/// Gets data (Column and row) of a cell number.
	/// </summary>
	/// <returns>The cell index.</returns>
	/// <param name="_number">Number.</param>
	public static Vector2 GetCellIndex (int _number)
	{
		int row = (int)((_number - 1) / 10f) + 1;
		int column = _number - ((int)(_number / 20f) * 20);

		if (column > 10)
			column = 11 - (column - 10);

		column = Mathf.Max (column, 1);

		return new Vector2 (column, row);
	}
}
